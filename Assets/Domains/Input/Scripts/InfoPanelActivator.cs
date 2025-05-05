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

        public bool onlyShowOnce;

        public bool hasBeenShown;


        [Tooltip("Optional offset from center of screen (Canvas space).")]
        public Vector2 screenOffset = Vector2.zero;

        [Header("Feedbacks")] public MMFeedbacks hidePanelFeedbacks;

        public MMFeedbacks showPanelFeedbacks;

        private GameObject _infoPanelInstance;

        public void ShowInteractablePrompt()
        {
            if (infoPanelPrefab == null) return;

            if (hasBeenShown && onlyShowOnce) return;

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

            var rectTransform = _infoPanelInstance.GetComponent<RectTransform>();
            if (rectTransform != null)
                rectTransform.anchoredPosition = screenOffset;
        }

        public void HideInteractablePrompt()
        {
            if (hasBeenShown && onlyShowOnce) return;
            if (_infoPanelInstance != null) _infoPanelInstance.SetActive(false);
        }

        public void Interact()
        {
            if (hasBeenShown && onlyShowOnce) return;

            if (!hasBeenShown && onlyShowOnce) hasBeenShown = true;
        } // Optional: leave empty if no interaction is needed
    }
}