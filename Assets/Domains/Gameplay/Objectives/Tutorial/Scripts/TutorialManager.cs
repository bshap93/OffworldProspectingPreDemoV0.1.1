using System.Collections.Generic;
using Domains.UI_Global.Events;
using MoreMountains.Tools;
using PixelCrushers.QuestMachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialManager : SerializedMonoBehaviour, MMEventListener<TutorialEvent>
{
    public Dictionary<TutorialEventType, string> tutorialMessageStrings;

    private QuestControl questControl;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        questControl = GetComponent<QuestControl>();
        if (questControl == null) Debug.LogError("QuestControl component not found.");
    }


    private void OnEnable()
    {
        this.MMEventStartListening();
    }

    private void OnDisable()
    {
        this.MMEventStopListening();
    }

    public void OnMMEvent(TutorialEvent eventType)
    {
        switch (eventType.EventType)
        {
            case TutorialEventType.PlayerUsedMoreInfo:
                SendPixelCrushersMessage(
                    tutorialMessageStrings[TutorialEventType.PlayerUsedMoreInfo]);
                break;
            case TutorialEventType.PlayerSoldInitialItems:
                SendPixelCrushersMessage(
                    tutorialMessageStrings[TutorialEventType.PlayerSoldInitialItems]);
                break;
        }
    }

    public void SendPixelCrushersMessage(string message)
    {
        if (questControl == null) return;

        questControl.SendToMessageSystem(message);
    }
}