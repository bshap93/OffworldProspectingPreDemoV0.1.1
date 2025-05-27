using CompassNavigatorPro;
using Domains.Gameplay.Objectives.Events;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveControllerHandler : MonoBehaviour, MMEventListener<ObjectiveEvent>
{
    public UnityEvent onObjectiveComplete;
    [SerializeField] private string objectiveId;
    [SerializeField] private CompassProPOI compassProPOI;

    public void HandleSetObjectiveActive()
    {
        
        if (compassProPOI != null)
        {
            compassProPOI.ToggleIndicatorVisibility(true);
            compassProPOI.ToggleCompassBarIconVisibility(true);
        }
    }

    public void HandleSetObjectiveComplete()
    {
        onObjectiveComplete?.Invoke();

        if (compassProPOI != null)
        {
            compassProPOI.ToggleIndicatorVisibility(false);
            compassProPOI.ToggleCompassBarIconVisibility(false);
        }
    }

    public void OnMMEvent(ObjectiveEvent eventType)
    {
        if (eventType.objectiveId == objectiveId)
        {
            if (eventType.type == ObjectiveEventType.ObjectiveActivated)
            {
                HandleSetObjectiveActive();
            }
            else if (eventType.type == ObjectiveEventType.ObjectiveCompleted)
            {
                HandleSetObjectiveComplete();
            }
        }
    }
    
    public void TriggerCompleteObjective()
    {
        ObjectiveEvent.Trigger(objectiveId, ObjectiveEventType.ObjectiveCompleted);
    }
    
    private void OnEnable()
    {
        this.MMEventStartListening();
    }
    
    private void OnDisable()
    {
        this.MMEventStopListening();
    }
}