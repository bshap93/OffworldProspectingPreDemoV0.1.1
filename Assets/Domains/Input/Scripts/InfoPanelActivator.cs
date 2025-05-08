using System.Collections;
using Domains.Gameplay.Mining.Scripts;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Domains.Input.Scripts
{
    public class InfoPanelActivator : MonoBehaviour, IInteractable
    {
        [Tooltip("Prefab to show when the object is looked at.")]
        public GameObject infoPanelPrefab;

        public Canvas canvas;

        [Tooltip("Optional offset from center of screen (Canvas space).")]
        public Vector2 screenOffset = Vector2.zero;

        [Header("Timeout Settings")] [Tooltip("Time in seconds before the panel auto-hides. Set to 0 for no timeout.")]
        public float panelTimeout = 5f;

        [Header("Feedbacks")] public MMFeedbacks hidePanelFeedbacks;

        public MMFeedbacks showPanelFeedbacks;
        private Coroutine _hideTimerCoroutine;

        private GameObject _infoPanelInstance;

        private void OnDisable()
        {
            // Make sure to clean up when disabled
            HideInfoPanel();
        }

        public void ShowInteractablePrompt()
        {
            if (infoPanelPrefab == null) return;

            if (_infoPanelInstance == null)
            {
                if (canvas == null)
                {
                    UnityEngine.Debug.LogWarning("No Canvas found in scene!");
                    return;
                }

                _infoPanelInstance = Instantiate(infoPanelPrefab, canvas.transform);
            }

            _infoPanelInstance.SetActive(true);
            showPanelFeedbacks?.PlayFeedbacks();

            var rectTransform = _infoPanelInstance.GetComponent<RectTransform>();
            if (rectTransform != null)
                rectTransform.anchoredPosition = screenOffset;

            // Start or restart the timeout timer
            StartPanelTimeout();
        }

        public void HideInteractablePrompt()
        {
            HideInfoPanel();
        }

        public void Interact()
        {
            // Optional: leave empty or implement any interaction behavior
        }

        public void ShowInfoPanel()
        {
            ShowInteractablePrompt(); // Fixed the recursive call
        }

        public void HideInfoPanel()
        {
            // Cancel any existing timeout
            if (_hideTimerCoroutine != null)
            {
                StopCoroutine(_hideTimerCoroutine);
                _hideTimerCoroutine = null;
            }

            if (_infoPanelInstance != null && _infoPanelInstance.activeSelf)
            {
                _infoPanelInstance.SetActive(false);
                hidePanelFeedbacks?.PlayFeedbacks();
            }
        }

        private void StartPanelTimeout()
        {
            // Only start timeout if a positive value is set
            if (panelTimeout <= 0) return;

            // Cancel any existing timeout
            if (_hideTimerCoroutine != null) StopCoroutine(_hideTimerCoroutine);

            // Start a new timeout
            _hideTimerCoroutine = StartCoroutine(HideAfterTimeout());
        }

        private IEnumerator HideAfterTimeout()
        {
            yield return new WaitForSeconds(panelTimeout);
            HideInfoPanel();
            _hideTimerCoroutine = null;
        }
    }
}