using System;
using Domains.Debug;
using Domains.Scene.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domains.UI_Global.Scripts
{
    public class MainMenuButton : MonoBehaviour
    {
        public enum MainMenuButtonType
        {
            NewGame,
            ContinueGame,
            Settings,
            QuitGame
        }

        [SerializeField] private MainMenuButtonType buttonType;
        [SerializeField] private string directSceneToLoad = "MainScene08"; // Emergency fallback
        [SerializeField] private bool useEventSystem = true;
        [SerializeField] private bool useDirectLoadingFallback = true;

        public bool asksForConfirmation;
        [SerializeField] private Button button;

        private bool _clickHandled;
        private float _clickTime;
        private SceneLoadManager _sceneLoadManager;

        private void Awake()
        {
            UnityEngine.Debug.Log($"[MainMenuButton] Awake: {buttonType}");

            // Find SceneLoadManager reference
            _sceneLoadManager = FindObjectOfType<SceneLoadManager>();
            if (_sceneLoadManager != null && !string.IsNullOrEmpty(_sceneLoadManager.sceneToLoad))
            {
                directSceneToLoad = _sceneLoadManager.sceneToLoad;
                UnityEngine.Debug.Log($"[MainMenuButton] Using scene from SceneLoadManager: {directSceneToLoad}");
            }

            // Get button component
            if (button == null)
            {
                button = GetComponent<Button>();
                if (button == null)
                {
                    UnityEngine.Debug.LogError($"[MainMenuButton] No Button component found on {gameObject.name}");
                    return;
                }
            }

            // Add listener
            button.onClick.RemoveAllListeners(); // Clear any existing listeners
            button.onClick.AddListener(OnClick);

            UnityEngine.Debug.Log($"[MainMenuButton] Button {gameObject.name} initialized with type {buttonType}");
        }

        private void Start()
        {
            if (buttonType == MainMenuButtonType.ContinueGame)
            {
                var hasSave = SceneLoadManager.HasGameSave();
                UnityEngine.Debug.Log($"[MainMenuButton] Continue button - HasSave: {hasSave}");

                button.interactable = hasSave;
            }
        }

        private void Update()
        {
            // Check for timeout on click
            if (_clickHandled && useDirectLoadingFallback && Time.time - _clickTime > 7f)
            {
                UnityEngine.Debug.LogWarning("[MainMenuButton] Click timeout exceeded, attempting direct load");
                _clickHandled = false; // Reset to prevent multiple loads

                // Emergency direct loading
                if (buttonType == MainMenuButtonType.NewGame || buttonType == MainMenuButtonType.ContinueGame)
                    EmergencyDirectLoad();
            }
        }

        public void OnClick()
        {
            UnityEngine.Debug.Log($"[MainMenuButton] OnClick: {buttonType}");

            // Prevent multiple processing of the same click
            if (_clickHandled)
            {
                UnityEngine.Debug.Log("[MainMenuButton] Click already being handled");
                return;
            }

            switch (buttonType)
            {
                case MainMenuButtonType.NewGame:
                    UnityEngine.Debug.Log("[MainMenuButton] New Game Button Clicked");

                    if (asksForConfirmation)
                    {
                        // Show confirmation dialog
                        if (useEventSystem) MainMenuEvent.Trigger(MainMenuEventType.NewGameAttempted);
                    }
                    else
                    {
                        _clickHandled = true;
                        _clickTime = Time.time;

                        DataReset.ClearAllSaveData();

                        if (useEventSystem)
                            MainMenuEvent.Trigger(MainMenuEventType.NewGameTriggered);
                        else
                            EmergencyDirectLoad();
                    }

                    break;

                case MainMenuButtonType.ContinueGame:
                    UnityEngine.Debug.Log("[MainMenuButton] Continue Game Button Clicked");

                    if (SceneLoadManager.HasGameSave())
                    {
                        _clickHandled = true;
                        _clickTime = Time.time;

                        if (useEventSystem)
                            MainMenuEvent.Trigger(MainMenuEventType.ContinueGameTriggered);
                        else
                            EmergencyDirectLoad();
                    }

                    break;

                case MainMenuButtonType.Settings:
                    if (useEventSystem) MainMenuEvent.Trigger(MainMenuEventType.SettingsOpenTriggered);
                    break;

                case MainMenuButtonType.QuitGame:
                    if (useEventSystem)
                        MainMenuEvent.Trigger(MainMenuEventType.QuitGameTriggered);
                    else
                        Application.Quit();
                    break;
            }
        }

        private void EmergencyDirectLoad()
        {
            UnityEngine.Debug.LogWarning($"[MainMenuButton] EMERGENCY: Direct loading scene {directSceneToLoad}");

            try
            {
                if (_sceneLoadManager != null)
                    _sceneLoadManager.LoadSceneDirect(directSceneToLoad);
                else
                    SceneManager.LoadScene(directSceneToLoad);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[MainMenuButton] CRITICAL: Direct load failed: {e.Message}");
            }
        }
    }
}