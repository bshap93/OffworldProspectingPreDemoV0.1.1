﻿using System.Linq;
using Digger.Modules.Runtime.Sources;
using Domains.Gameplay.Mining.Scripts;
using Domains.Player.Camera;
using Domains.Player.Events;
using Domains.Player.Scripts;
using Domains.Scripts_that_Need_Sorting;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.Gameplay.Tools.ToolSpecifics
{
    public class ShovelTool : BaseDiggerUsingTool, MMEventListener<UpgradeEvent>
    {
        [Header("Feedbacks")] public MMFeedbacks diggingFeedbacks;

        public GameObject debrisEffectPrefab;

        [Header("Material")] [Header("Material Settings")]
        public Material currentMaterial;

        [Header("Shovel Models")] [SerializeField]
        private GameObject shovelObject;

        [SerializeField] private GameObject shovelGripObject;
        [SerializeField] private GameObject shovelMidObject;

        private void Awake()
        {
            digger = FindFirstObjectByType<DiggerMasterRuntime>();
            playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        }

        private void OnEnable()
        {
            this.MMEventStartListening();
            // Reapply current material when enabled
            if (currentMaterial != null)
            {
                SetCurrentMaterial(currentMaterial);
                UnityEngine.Debug.Log($"{GetType().Name} enabled - reapplied material: {currentMaterial.name}");
            }
        }

        private void OnDisable()
        {
            this.MMEventStopListening();
        }


        public void OnMMEvent(UpgradeEvent eventType)
        {
            if (eventType.EventType == UpgradeEventType.ShovelMiningSizeSet)
                SetDiggerUsingToolEffectSize(eventType.EffectValue, eventType.EffectValue2);
        }

        public void SetCurrentMaterial(Material material)
        {
            currentMaterial = material;
            if (shovelObject != null)
                shovelObject.GetComponent<Renderer>().material = currentMaterial;
            if (shovelGripObject != null)
                shovelGripObject.GetComponent<Renderer>().material = currentMaterial;
            if (shovelMidObject != null)
                shovelMidObject.GetComponent<Renderer>().material = currentMaterial;
        }


        public override void UseTool(RaycastHit hit)
        {
            lastHit = hit;
            PerformToolAction();
        }


        public override void PerformToolAction()
        {
            var detectedTextureIndex = terrainLayerDetector.GetTextureIndex(lastHit, out _);

// Reject early if not in allowed textures (raw index)
            if (!CanInteractWithTextureIndex(detectedTextureIndex)) return;

// Determine final texture to dig into
            var textureIndex = GetTerrainLayerBasedOnDepthAndOverrides(detectedTextureIndex, lastHit.point.y);


            if (Time.time < lastDigTime + miningCooldown)
                return;

            lastDigTime = Time.time;

            if (playerInteraction == null || digger == null)
                return;


            var notPlayerMask = ~playerInteraction.playerLayerMask;
            if (!Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out var hit,
                    diggerUsingRange,
                    notPlayerMask))
                return;

            // Cache hit for external access
            lastHit = hit;

            // Interact
            if (CanInteractWithObject(hit.collider.gameObject))
            {
                // Call IInteractable if implemented
                hit.collider.GetComponent<IInteractable>()?.Interact();


                var minable = hit.collider.GetComponent<IMinable>();
                if (minable != null)
                {
                    minable.MinableFailHit(hit.point);
                    moveToolDespiteFailHitFeedbacks?.PlayFeedbacks();
                    CameraEffectEvent.Trigger(CameraEffectEventType.ShakeCameraPosition, 0.2f);
                    return;
                }
            }

            // Return after triggering failed mining feedbacks, and before digging
            if (!allowedTerrainTextureIndices.Contains(detectedTextureIndex)) return;

            GameObject prefabToUse = null;
            foreach (var terDigPrefab in TerrainController.Instance.terrainBehavior.terrainDigParticlePrefabs)
                if (terDigPrefab.terrainLayerIndex == detectedTextureIndex)
                {
                    prefabToUse = terDigPrefab.primaryPrefab;
                    break;
                }

            if (prefabToUse != null)
            {
                UnityEngine.Debug.Log("Prefab to use found: " + prefabToUse.name);

                TriggerDebrisEffect(prefabToUse, hit);
            }
            else
            {
                TriggerDebrisEffect(debrisEffectPrefab, hit);
            }


            // Feedback trigger (from PerformToolAction, not MMFeedbacks directly)
            if (diggingFeedbacks != null) diggingFeedbacks.PlayFeedbacks(hit.point);

            // Dig!
            var digPosition = hit.point + mainCamera.transform.forward * 0.3f;

            if (EditAsynchronously)
                digger.ModifyAsyncBuffured(digPosition, brush, Action, textureIndex, effectOpacity, effectRadius,
                    stalagmiteHeight);
            else
                digger.Modify(digPosition, brush, Action, textureIndex, effectOpacity, effectRadius);

            // FuelEvent.Trigger(FuelEventType.ConsumeFuel, 2f, PlayerFuelManager.MaxFuelPoints);
        }
    }
}