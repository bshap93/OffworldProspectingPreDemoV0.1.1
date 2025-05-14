using Domains.Debug;
using Domains.Debug.Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class NewGameButton : MonoBehaviour, MMEventListener<DataSaveEvent>
{
    [SerializeField] private MMFeedbacks loadNewGameFeedbacks;

    private void OnEnable()
    {
        this.MMEventStartListening();
    }

    private void OnDisable()
    {
        this.MMEventStopListening();
    }

    public void OnMMEvent(DataSaveEvent eventType)
    {
        if (eventType.EventType == DataSaveEventType.DataResetFinished) loadNewGameFeedbacks?.PlayFeedbacks();
    }

    public void OnClick()
    {
        DataReset.ClearAllSaveData();
    }
}