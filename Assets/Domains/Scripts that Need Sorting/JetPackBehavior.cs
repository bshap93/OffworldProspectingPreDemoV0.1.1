using Domains.Player.Events;
using Domains.Player.Scripts;
using UnityEngine;

namespace Domains.Scripts_that_Need_Sorting
{
    public class JetPackBehavior : MonoBehaviour
    {
        private float jetPackButtonHoldTime;
        private Vector3 smoothDampVelocity = Vector3.zero;


        public void UpdateHoldTime(float deltaTime)
        {
            jetPackButtonHoldTime += deltaTime;
        }

        public void ResetHoldTime()
        {
            jetPackButtonHoldTime = 0f;
        }

        public float GetHoldTime()
        {
            return jetPackButtonHoldTime;
        }


        public void JetPackBehaviorMethod()
        {
            FuelEvent.Trigger(FuelEventType.ConsumeFuel, 5f, PlayerFuelManager.MaxFuelPoints);
        }

        public Vector3 ApplyJetpackLift(Vector3 currentVelocity, Vector3 upDirection, float targetHeight,
            float duration, float speedMultiplier)
        {
            return Vector3.SmoothDamp(
                currentVelocity,
                targetHeight * upDirection,
                ref smoothDampVelocity,
                duration
            ) * speedMultiplier;
        }
    }
}