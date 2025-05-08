using Domains.Scene.MainMenu;
using MoreMountains.Tools;
using UnityEngine;

public class SettingsPanelsMainMenu : MonoBehaviour, MMEventListener<MainMenuEvent>
{
    private CanvasGroup settingsPanelCanvasGroup;

    private void Awake()
    {
        settingsPanelCanvasGroup = GetComponent<CanvasGroup>();
        if (settingsPanelCanvasGroup == null)
            Debug.LogError("SettingsPanelsMainMenu: No CanvasGroup found on this GameObject");
    }

    private void OnEnable()
    {
        this.MMEventStartListening();
    }

    private void OnDisable()
    {
        this.MMEventStopListening();
    }

    public void OnMMEvent(MainMenuEvent eventType)
    {
        switch (eventType.EventType)
        {
            case MainMenuEventType.SettingsOpenTriggered:
                OpenSettingsPanel();
                break;
            case MainMenuEventType.SettingsCloseTriggered:
                CloseSettingsPanel();
                break;
        }
    }

    public void TriggerClose()
    {
        MainMenuEvent.Trigger(MainMenuEventType.SettingsCloseTriggered);
    }

    public void TriggerOpen()
    {
        MainMenuEvent.Trigger(MainMenuEventType.SettingsOpenTriggered);
    }


    private void OpenSettingsPanel()
    {
        // Logic to open settings panel
        settingsPanelCanvasGroup.alpha = 1;
        settingsPanelCanvasGroup.interactable = true;
        settingsPanelCanvasGroup.blocksRaycasts = true;
    }

    private void CloseSettingsPanel()
    {
        // Logic to close settings panel
        settingsPanelCanvasGroup.alpha = 0;
        settingsPanelCanvasGroup.interactable = false;
        settingsPanelCanvasGroup.blocksRaycasts = false;
    }
}