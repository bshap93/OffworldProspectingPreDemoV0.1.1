using System.Collections.Generic;
using Domains.Input.Scripts;
using Domains.Scene.Events;
using Domains.UI_Global.Events;
using Domains.UI_Global.Scripts;
using UnityEngine;

namespace Domains.Gameplay.Managers.Scripts
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;

        [SerializeField] private AudioSource uiButtonAudioSource;

        private List<AudioSource> _audioSources = new();

        private bool _isPaused;

        private void Awake()
        {
            Instance = this;
        }


        private void Update()
        {
            if (CustomInputBindings.IsPausePressed() && !PlayerUIManager.Instance.uiIsOpen)
            {
                _isPaused = !_isPaused;
                Time.timeScale = _isPaused ? 0 : 1;
                if (_isPaused)
                {
                    _audioSources = new List<AudioSource>(FindObjectsByType<AudioSource>(FindObjectsSortMode.None));
                    foreach (var audioSource in _audioSources)
                        if (audioSource != null && audioSource != uiButtonAudioSource)
                            audioSource.Pause();
                }
                else
                {
                    foreach (var audioSource in _audioSources)
                        if (audioSource != null && audioSource != uiButtonAudioSource)
                            audioSource.UnPause();
                }


                SceneEvent.Trigger(SceneEventType.TogglePauseScene);
            }
            else if (CustomInputBindings.IsPausePressed())
            {
                UIEvent.Trigger(UIEventType.CloseVendorConsole);
                UIEvent.Trigger(UIEventType.CloseFuelConsole);
                UIEvent.Trigger(UIEventType.CloseUI);
                UIEvent.Trigger(UIEventType.CloseBriefing);
            }
        }

        public bool IsPaused()
        {
            return _isPaused;
        }
    }
}