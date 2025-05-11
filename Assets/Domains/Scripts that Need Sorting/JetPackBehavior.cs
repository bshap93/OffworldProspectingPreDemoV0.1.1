using Domains.Player.Events;
using Domains.Player.Scripts;
using Rewired;
using UnityEngine;

namespace Domains.Scripts_that_Need_Sorting
{
    public class JetPackBehavior : MonoBehaviour
    {
        [SerializeField] private int playerId;
        [SerializeField] private float activationHoldTime = 0.3f; // How long to hold jump before jetpack activates

        private readonly float jetpackCooldown = 0.36f;
        private float jetPackButtonHoldTime;
        private float nextJetpackActionTime;
        private Rewired.Player rewiredPlayer;
        private Vector3 smoothDampVelocity = Vector3.zero;

        private void Awake()
        {
            rewiredPlayer = ReInput.players.GetPlayer(playerId);
            if (rewiredPlayer == null)
                UnityEngine.Debug.LogError($"Rewired player with ID {playerId} not found.");
        }

        private void Update()
        {
            // Track if Jump button is being held
            if (rewiredPlayer.GetButton("Jump"))
                jetPackButtonHoldTime += Time.deltaTime;
            else
                // Reset hold time when button is released
                jetPackButtonHoldTime = 0f;
        }

        public void TryTriggerJetPackEffect()
        {
            if (Time.time >= nextJetpackActionTime)
            {
                JetPackBehaviorMethod(); // formerly only called by MMF Player
                nextJetpackActionTime = Time.time + jetpackCooldown;
            }
        }


        public void UpdateHoldTime(float deltaTime)
        {
        }

        public void ResetHoldTime()
        {
            jetPackButtonHoldTime = 0f;
        }

        public float GetHoldTime()
        {
            return jetPackButtonHoldTime;
        }

        public bool ShouldActivateJetpack()
        {
            return rewiredPlayer.GetButton("Jump") && jetPackButtonHoldTime >= activationHoldTime;
        }


        public void JetPackBehaviorMethod()
        {
            FuelEvent.Trigger(FuelEventType.ConsumeFuel, 5f, PlayerFuelManager.MaxFuelPoints);
        }

        public Vector3 ApplyJetpackLift(Vector3 currentVelocity, Vector3 upDirection, float targetHeight,
            float duration, float speedMultiplier)
        {
            var lift = upDirection * targetHeight * speedMultiplier;
            return Vector3.Lerp(currentVelocity, lift, 0.5f); // Blends in smoothly, but not fully SmoothDamp
        }
    }
}