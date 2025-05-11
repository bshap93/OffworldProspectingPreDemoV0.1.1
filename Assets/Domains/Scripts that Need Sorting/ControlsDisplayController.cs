using Domains.Input.Scripts;
using Domains.Player.Progression;
using UnityEngine;

namespace Domains.Scripts_that_Need_Sorting
{
    public class ControlsDisplayController : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private MyRewiredInputManager inputManager;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                UnityEngine.Debug.LogError("ControlsDisplayController: No CanvasGroup found on this GameObject.");
            ShowControls();
            inputManager = MyRewiredInputManager.Instance;

            if (inputManager == null)
                UnityEngine.Debug.LogError("ControlsDisplayController: No MyRewiredInputManager found in the scene.");
        }

        // Update is called once per frame
        private void Update()
        {
            if (inputManager.IsGetMoreInfoPressed())
                ShowControls();
            else if (ProgressionManager.TutorialFinished)
                HideControls();
        }

        private void ShowControls()
        {
            if (canvasGroup != null) canvasGroup.alpha = 1f;
        }

        private void HideControls()
        {
            if (canvasGroup != null) canvasGroup.alpha = 0f;
        }
    }
}