using Domains.Scene.MainMenu;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private MainMenuButtonType buttonType;

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
                MainMenuEvent.Trigger(MainMenuEventType.NewGameAttempted);
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