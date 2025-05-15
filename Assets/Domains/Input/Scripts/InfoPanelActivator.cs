﻿using Domains.UI_Global.Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Domains.Input.Scripts
{
    public class InfoPanelActivator : MonoBehaviour, MMEventListener<UIEvent>
    {
        [Tooltip("Prefab to show when the object is looked at.")]
        public GameObject infoPanelPrefab;

        public Canvas canvas;

        [Tooltip("Optional offset from center of screen (Canvas space).")]
        public Vector2 screenOffset = Vector2.zero;

        [Header("Feedbacks")] public MMFeedbacks hidePanelFeedbacks;

        public UnityEvent onShowPanel;

        public UnityEvent onHidePanel;

        public MMFeedbacks showPanelFeedbacks;
        // private Coroutine _hideTimerCoroutine;

        private GameObject _infoPanelInstance;

        private bool _isPanelVisible;

        private void OnEnable()
        {
            this.MMEventStartListening();
        }

        private void OnDisable()
        {
            // Make sure to clean up when disabled
            HideInfoPanel();
            this.MMEventStopListening();
        }


        public void OnMMEvent(UIEvent eventType)
        {
            if (eventType.EventType == UIEventType.CloseInfoPanel) HideInfoPanel();
        }


        public void ToggleInfoPanel()
        {
            if (!_isPanelVisible)
            {
                UnityEngine.Debug.Log("InfoPanel is not instantiated yet.");
                ShowInfoPanel();
            }
            else
            {
                HideInfoPanel();
            }
        }

        public void ShowInfoPanel()
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
            _isPanelVisible = true;
            UIEvent.Trigger(UIEventType.OpenInfoPanel);
            showPanelFeedbacks?.PlayFeedbacks();
            onShowPanel?.Invoke();

            var rectTransform = _infoPanelInstance.GetComponent<RectTransform>();
            if (rectTransform != null)
                rectTransform.anchoredPosition = screenOffset;
        }


        public void HideInfoPanel()
        {
            if (_infoPanelInstance != null && _infoPanelInstance.activeSelf)
            {
                _infoPanelInstance.SetActive(false);
                _isPanelVisible = false;
                UIEvent.Trigger(UIEventType.CloseInfoPanel);
                hidePanelFeedbacks?.PlayFeedbacks();
                onHidePanel?.Invoke();
            }
        }
    }
}