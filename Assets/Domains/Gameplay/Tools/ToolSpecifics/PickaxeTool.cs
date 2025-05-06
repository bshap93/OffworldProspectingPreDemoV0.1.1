using System;
using System.Collections;
using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using Domains.Gameplay.Mining.Scripts;
using Domains.Player.Events;
using Domains.Player.Scripts;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Domains.Gameplay.Tools.ToolSpecifics
{
    public class PickaxeTool : BaseDiggerUsingTool, MMEventListener<UpgradeEvent>
    {
        [Header("Stat Settings")] public int hardnessCanBreak;

        [SerializeField] private float delayBeforeDigging = 0.1f;

        [SerializeField] private MMFeedbacks firstHitFeedbacks;
        [SerializeField] private MMFeedbacks secondHitFeedbacks;
        [Header("Debris Effects")] public GameObject debrisEffectFirstHitPrefab;

        [SerializeField] private float firstHitEffectOpacity;
        [SerializeField] private float firstHitEffectRadius;

        public GameObject debrisEffectSecondHitPrefab;

        [FormerlySerializedAs("diggingFeedbacks")] [SerializeField]
        private MMFeedbacks pickaxeBehavior;

        public Material currentMaterial;

        [Header("Hit Number Logic")] [SerializeField]
        private readonly float hitThresholdDistance = 0.5f; // adjust as needed

        // Track active coroutine to prevent multiple digs happening at once
        private Coroutine activeDigCoroutine;

        // Flag to prevent race conditions
        private bool isDigging;

        private Vector3 lastHitPosition;
        private MeshRenderer meshRenderer;
        private int terrainHitCount;

        private void Awake()
        {
            digger = FindFirstObjectByType<DiggerMasterRuntime>();
            playerInteraction = FindFirstObjectByType<PlayerInteraction>();
            meshRenderer = GetComponent<MeshRenderer>();
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

            // Reset state when tool is enabled
            isDigging = false;
            terrainHitCount = 0;
            if (activeDigCoroutine != null)
            {
                StopCoroutine(activeDigCoroutine);
                activeDigCoroutine = null;
            }
        }

        private void OnDisable()
        {
            this.MMEventStopListening();

            // Cancel any ongoing digging when tool is disabled
            if (activeDigCoroutine != null)
            {
                StopCoroutine(activeDigCoroutine);
                activeDigCoroutine = null;
            }

            isDigging = false;
        }

        public void OnMMEvent(UpgradeEvent eventType)
        {
            if (eventType.EventType == UpgradeEventType.PickaxeMiningSizeSet)
                SetDiggerUsingToolEffectSize(eventType.EffectValue, eventType.EffectValue2);
        }

        public void SetCurrentMaterial(Material material)
        {
            currentMaterial = material;
            if (meshRenderer != null)
                meshRenderer.material = currentMaterial;
        }

        public override void UseTool(RaycastHit hit)
        {
            lastHit = hit;
            PerformToolAction();
        }

        public override void PerformToolAction()
        {
            // Prevent multiple simultaneous digging operations
            if (isDigging)
                return;

            try
            {
                // Check cooldown
                if (Time.time < lastDigTime + miningCooldown)
                    return;

                // Update last dig time
                lastDigTime = Time.time;

                // Validate core references
                if (playerInteraction == null || digger == null)
                    return;

                // Confirm we have a valid ray hit
                var notPlayerMask = ~playerInteraction.playerLayerMask;
                if (!Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out var hit,
                        diggerUsingRange, notPlayerMask))
                    return;

                // Cache hit for external access
                lastHit = hit;

                // Get texture index from hit
                var detectedTextureIndex = terrainLayerDetector.GetTextureIndex(lastHit, out _);

                // Check if we're hitting a minable object
                var isMinableObject = CanInteractWithObject(lastHit.collider.gameObject);

                // Check if we're hitting valid terrain
                var isTerrain = detectedTextureIndex >= 0;
                var isValidTerrain = CanInteractWithTextureIndex(detectedTextureIndex);

                // Exit early if we're not hitting anything valid
                if (!isMinableObject && (!isTerrain || !isValidTerrain))
                    return;

                // Handle object interaction first (ore, rocks, etc.)
                if (isMinableObject) HandleObjectInteraction(hit);

                // Exit if not a valid terrain texture
                if (!isValidTerrain)
                    return;

                // Get the actual texture index based on depth and overrides
                var textureIndex = GetTerrainLayerBasedOnDepthAndOverrides(detectedTextureIndex, lastHit.point.y);

                // Handle terrain digging
                HandleTerrainDigging(hit, textureIndex);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Pickaxe error: {ex.Message}\n{ex.StackTrace}");
                // Reset state on error
                isDigging = false;
            }
        }

        private void HandleObjectInteraction(RaycastHit hit)
        {
            // Call IInteractable if implemented
            hit.collider.GetComponent<IInteractable>()?.Interact();

            // Handle minable objects
            var minable = hit.collider.GetComponent<IMinable>();
            if (minable != null)
            {
                if (minable.GetCurrentMinableHardness() <= hardnessCanBreak)
                {
                    minable.MinableMineHit();
                    moveToolDespiteFailHitFeedbacks?.PlayFeedbacks();
                }
                else
                {
                    minable.MinableFailHit(hit.point);
                    moveToolDespiteFailHitFeedbacks?.PlayFeedbacks();
                }
            }
        }

        private void HandleTerrainDigging(RaycastHit hit, int textureIndex)
        {
            // Distance check: is this close enough to the last hit?
            if (terrainHitCount > 0 && Vector3.Distance(hit.point, lastHitPosition) > hitThresholdDistance)
                terrainHitCount = 0;

            // Store current hit
            lastHitPosition = hit.point;
            terrainHitCount++;

            // First hit is different - smaller impression
            if (terrainHitCount == 1)
            {
                var digPositionFirst = hit.point + mainCamera.transform.forward * 0.3f;

                // Set digging flag
                isDigging = true;

                // Start dig coroutine and track it
                activeDigCoroutine = StartCoroutine(Dig(digPositionFirst, textureIndex, firstHitEffectOpacity,
                    firstHitEffectRadius, BrushType.Stalagmite));

                // Trigger debris effect for first hit
                TriggerDebrisEffect(debrisEffectFirstHitPrefab, hit);

                // Play first hit feedback
                firstHitFeedbacks?.PlayFeedbacks(hit.point);

                return;
            }

            // Second hit breaks the terrain
            if (terrainHitCount >= 2)
            {
                UnityEngine.Debug.Log("Second hit - Breaking terrain");

                // Trigger second hit debris effect
                TriggerDebrisEffect(debrisEffectSecondHitPrefab, hit);

                // Play second hit feedback
                secondHitFeedbacks?.PlayFeedbacks(hit.point);

                // Reset hit count
                terrainHitCount = 0;

                var digPosition = hit.point + mainCamera.transform.forward * 0.3f;

                // Set digging flag
                isDigging = true;

                // Start full dig coroutine and track it
                activeDigCoroutine = StartCoroutine(Dig(digPosition, textureIndex, effectOpacity, effectRadius));
            }
        }

        // Fixed dig coroutine with proper cleanup
        private IEnumerator Dig(Vector3 digPosition, int textureIndex,
            float effectOpacityLoc, float effectRadiusLoc, BrushType brushLoc = BrushType.Sphere)
        {
            // Wait for delay
            yield return new WaitForSeconds(delayBeforeDigging);
            try
            {
                // Check if digger reference is still valid
                if (digger == null)
                {
                    UnityEngine.Debug.LogError("Digger reference lost");
                    isDigging = false;
                    yield break;
                }

                // Perform the actual digging operation
                try
                {
                    if (EditAsynchronously)
                        // Use the async buffered method with proper parameters
                        digger.ModifyAsyncBuffured(
                            digPosition,
                            brushLoc,
                            Action,
                            textureIndex,
                            effectOpacityLoc,
                            effectRadiusLoc,
                            stalagmiteHeight,
                            true
                        );
                    else
                        // Use the synchronous method
                        digger.Modify(
                            digPosition,
                            brushLoc,
                            Action,
                            textureIndex,
                            effectOpacityLoc,
                            effectRadiusLoc,
                            stalagmiteHeight,
                            true
                        );
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"Dig operation error: {ex.Message}\n{ex.StackTrace}");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Dig coroutine error: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                // Always reset the digging flag when done
                isDigging = false;
                activeDigCoroutine = null;
            }

            // Wait a frame to ensure digging operation had time to process
            yield return null;
        }
    }
}