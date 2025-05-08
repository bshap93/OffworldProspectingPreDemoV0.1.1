using Domains.Scene.MainMenu;
using UnityEngine;
using UnityEngine.UI;
// Change this to use UnityEngine.UI

namespace Domains.UI_Global.Scripts
{
    public class MainMenuButton : MonoBehaviour
    {
        // Make this public so it's easier to debug
        public enum MainMenuButtonType
        {
            NewGame,
            ContinueGame,
            Settings,
            QuitGame
        }

        [SerializeField] private MainMenuButtonType buttonType;

        public bool asksForConfirmation;
        [SerializeField] private Button button; // This should now use UnityEngine.UI.Button

        // Add debug logging to help diagnose the issue
        private void Awake()
        {
            UnityEngine.Debug.Log($"MainMenuButton Awake: {buttonType}");

            // Check if button component exists
            if (button == null)
            {
                button = GetComponent<Button>();
                if (button == null)
                {
                    UnityEngine.Debug.LogError($"No Button component found on {gameObject.name}");
                    return;
                }
            }

            // Add listener programmatically to ensure it's connected
            button.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            UnityEngine.Debug.Log($"MainMenuButton Start: {buttonType}, Has Save: {SceneLoadManager.HasGameSave()}");

            if (buttonType == MainMenuButtonType.ContinueGame)
            {
                var hasSave = SceneLoadManager.HasGameSave();
                UnityEngine.Debug.Log($"Continue button - HasSave: {hasSave}");

                // Use button.interactable instead of SetEnabled
                if (!hasSave)
                    button.interactable = false;
                else
                    button.interactable = true;
            }
        }

        public void OnClick()
        {
            UnityEngine.Debug.Log($"MainMenuButton OnClick: {buttonType}");

            switch (buttonType)
            {
                case MainMenuButtonType.NewGame:
                    UnityEngine.Debug.Log("New Game Button Clicked");
                    if (asksForConfirmation)
                        // Show confirmation dialog
                        MainMenuEvent.Trigger(MainMenuEventType.NewGameAttempted);
                    else
                        MainMenuEvent.Trigger(MainMenuEventType.NewGameTriggered);
                    break;
                case MainMenuButtonType.ContinueGame:
                    UnityEngine.Debug.Log("Continue Game Button Clicked");
                    MainMenuEvent.Trigger(MainMenuEventType.ContinueGameTriggered);
                    break;
                case MainMenuButtonType.Settings:
                    MainMenuEvent.Trigger(MainMenuEventType.SettingsOpenTriggered);
                    break;
                case MainMenuButtonType.QuitGame:
                    MainMenuEvent.Trigger(MainMenuEventType.QuitGameTriggered);
                    break;
            }
        }
    }
}