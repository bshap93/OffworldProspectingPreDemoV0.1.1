using Domains.Scene.MainMenu;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private MainMenuButtonType buttonType;

    public bool asksForConfirmation;
    [SerializeField] private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (buttonType == MainMenuButtonType.ContinueGame)
            if (SceneLoadManager.HasGameSave() == false)
                button.SetEnabled(false);
    }

    public void OnClick()
    {
        switch (buttonType)
        {
            case MainMenuButtonType.NewGame:
                Debug.Log("New Game Button Clicked");
                if (asksForConfirmation)
                    // Show confirmation dialog
                    MainMenuEvent.Trigger(MainMenuEventType.NewGameAttempted);
                else
                    MainMenuEvent.Trigger(MainMenuEventType.NewGameTriggered);
                break;
            case MainMenuButtonType.ContinueGame:
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

    private enum MainMenuButtonType
    {
        NewGame,
        ContinueGame,
        Settings,
        QuitGame
    }
}