using Domains.Scripts_that_Need_Sorting;
using Lightbug.CharacterControllerPro.Core;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Domains.Gameplay.Mining.Scripts
{
    internal class FootstepManager : MonoBehaviour
    {
        [Header("Footsteps")] [SerializeField] private MMFeedbacks terrainFootstepFeedbacks;

        [SerializeField] private MMFeedbacks chunkFootstepFeedbacks;
        [SerializeField] private MMFeedbacks defaultFootstepFeedbacks;
        [SerializeField] private float baseStepInterval = 0.5f;

        [FormerlySerializedAs("DownTextureDetector")]
        public TerrainLayerDetector downTerrainLayerDetector;

        [SerializeField] private CharacterActor characterActor;

        private float _footstepInterval;

        private float _footstepTimer;

        private bool _wasMovingLastFrame;


        private void Awake()
        {
            if (characterActor == null)
                characterActor = FindFirstObjectByType<CharacterActor>();
        }

        private void Update()
        {
            UpdateFootsteps(Time.deltaTime);
        }

        private void UpdateFootsteps(float dt)
        {
            var isMoving = characterActor.IsGrounded && characterActor.PlanarVelocity.magnitude > 0.01f;

            if (isMoving)
            {
                // Dynamically scale interval
                var speed = characterActor.PlanarVelocity.magnitude;
                _footstepInterval = baseStepInterval / Mathf.Max(speed, 0.1f);
                _footstepInterval = Mathf.Clamp(_footstepInterval, baseStepInterval * 0.7f, baseStepInterval * 1.3f);

                // Trigger first footstep as soon as movement begins
                if (!_wasMovingLastFrame)
                    _footstepTimer = _footstepInterval;

                // Run step timer
                if (_footstepTimer >= _footstepInterval)
                {
                    PlayFootstepFeedback();
                    _footstepTimer = 0f;
                }
                else
                    // ERROR - no dt in context
                {
                    _footstepTimer += dt;
                }
            }
            else
            {
                _footstepTimer = 0f;
            }

            _wasMovingLastFrame = isMoving;
        }


        private void PlayFootstepFeedback()
        {
            switch (downTerrainLayerDetector.textureIndex)
            {
                case 1: // Chunk terrain from Digger
                    chunkFootstepFeedbacks?.PlayFeedbacks();
                    break;

                case >= 0: // Terrain (index 0, 2, 3, etc.)
                    terrainFootstepFeedbacks?.PlayFeedbacks();
                    break;

                case -1: // Meshes, non-terrain
                default:
                    defaultFootstepFeedbacks?.PlayFeedbacks();
                    break;
            }
        }
    }
}