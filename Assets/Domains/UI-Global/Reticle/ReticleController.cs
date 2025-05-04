using Domains.Gameplay.Equipment.Scripts;
using Domains.Gameplay.Mining.Scripts;
using Domains.Gameplay.Tools.ToolSpecifics;
using Domains.Player.Scripts;
using Domains.Scripts_that_Need_Sorting;
using UnityEngine;
using UnityEngine.UI;

namespace Domains.UI_Global.Reticle
{
    public class ReticleController : MonoBehaviour
    {
        [Header("Reticle UI")]
        public Image reticle;
        
        [Header("Reticle States")]
        public ReticleState defaultState;
        public ReticleState interactableState;
        public ReticleState mineableState;
        public ReticleState switchToolState;
        public ReticleState validTerrainState;
        
        [Header("References")]
        public PlayerInteraction playerInteraction;
        public TerrainLayerDetector terrainLayerDetector;
        
        private ReticleState currentState;
        
        public void UpdateReticle(RaycastHit? hit, bool terrainBlocking)
        {
            ReticleState targetState = defaultState;
            
            if (hit.HasValue)
            {
                var currentTool = PlayerEquipment.Instance.CurrentToolComponent;
                var terrainIndex = terrainLayerDetector.textureIndex;
                
                // Check for interactable components
                var interactable = hit.Value.collider.GetComponent<IInteractable>();
                var minable = hit.Value.collider.GetComponent<IMinable>();
                
                if (interactable != null && !terrainBlocking)
                {
                    targetState = interactableState;
                }
                else if (minable != null && !terrainBlocking)
                {
                    // Check if current tool can mine this object
                    var pickaxe = currentTool as PickaxeTool;
                    if (pickaxe != null && pickaxe.hardnessCanBreak >= minable.GetCurrentMinableHardness())
                    {
                        targetState = mineableState;
                    }
                    else
                    {
                        // Tool can't mine this, suggest switching
                        targetState = switchToolState;
                    }
                }
                else if (currentTool != null && terrainIndex >= 0)
                {
                    // Check if current tool can dig this terrain
                    if (currentTool.CanInteractWithTextureIndex(terrainIndex))
                    {
                        targetState = validTerrainState;
                    }
                    else
                    {
                        // Check if another tool can dig this terrain
                        foreach (var tool in PlayerEquipment.Instance.Tools)
                        {
                            if (tool != currentTool && tool.CanInteractWithTextureIndex(terrainIndex))
                            {
                                targetState = switchToolState;
                                break;
                            }
                        }
                    }
                }
            }
            
            ApplyReticleState(targetState);
        }
        
        private void ApplyReticleState(ReticleState state)
        {
            if (state != currentState)
            {
                currentState = state;
                if (state != null)
                {
                    reticle.sprite = state.reticleSprite;
                    reticle.color = state.reticleColor;
                }
                else
                {
                    reticle.sprite = defaultState.reticleSprite;
                    reticle.color = defaultState.reticleColor;
                }
            }
        }
    }
}