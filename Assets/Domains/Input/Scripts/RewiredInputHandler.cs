using Rewired;
using ThirdParty.Character_Controller_Pro.Implementation.Scripts.Inputs.InputHandler;
using UnityEngine;

namespace Domains.Input.Scripts
{
    public class RewiredInputHandler : InputHandler
    {
        private readonly MyRewiredInputManager inputManager;
        private readonly int playerId;

        public RewiredInputHandler(int playerId = 0)
        {
            this.playerId = playerId;
            inputManager = MyRewiredInputManager.Instance;
        }

        public override bool GetBool(string actionName)
        {
            switch (actionName)
            {
                case "Jump":
                    return ReInput.players.GetPlayer(playerId).GetButton("Jump");
                case "Run":
                    return ReInput.players.GetPlayer(playerId).GetButton("SprintPressed");
                case "Interact":
                    return ReInput.players.GetPlayer(playerId).GetButton("Interact");
                case "Jet Pack":
                    return false;
                // return ReInput.players.GetPlayer(playerId).GetButton("JetPack");
                case "Dash":
                    return false;
                // return ReInput.players.GetPlayer(playerId).GetButton("Dash");
                case "Crouch":
                    return ReInput.players.GetPlayer(playerId).GetButton("Crouch");
                default:
                    return false;
            }
        }

        public override float GetFloat(string actionName)
        {
            switch (actionName)
            {
                // case "Pitch":
                //     float pitch = ReInput.players.GetPlayer(playerId).GetAxis("AimVertical");
                //     return inputManager.InvertYAxis ? -pitch : pitch;
                // case "Roll":
                //     return ReInput.players.GetPlayer(playerId).GetAxis("AimHorizontal");
                default:
                    return 0f;
            }
        }

        public override Vector2 GetVector2(string actionName)
        {
            switch (actionName)
            {
                case "Movement":
                    return new Vector2(
                        ReInput.players.GetPlayer(playerId).GetAxis("MoveLeftRight"),
                        ReInput.players.GetPlayer(playerId).GetAxis("MoveForwardBack")
                    );
                case "Camera":
                    var cameraAim = new Vector2(
                        ReInput.players.GetPlayer(playerId).GetAxis("AimHorizontal"),
                        ReInput.players.GetPlayer(playerId).GetAxis("AimVertical")
                    );

                    // Apply Y inversion if needed
                    if (inputManager.InvertYAxis)
                        cameraAim.y = -cameraAim.y;

                    return cameraAim;
                default:
                    return Vector2.zero;
            }
        }
    }
}