using HighlightPlus;
using UnityEngine;

public class HighlightEffectController : MonoBehaviour
{
    [SerializeField] public string targetID;
    private HighlightEffect highlightEffect;
    private HighlightTrigger highlightTrigger;

    private void Awake()
    {
        highlightEffect = GetComponent<HighlightEffect>();
        highlightTrigger = GetComponent<HighlightTrigger>();

        if (highlightEffect == null)
        {
            Debug.LogError("HighlightEffect component not found on this GameObject.");
            return;
        }

        if (highlightTrigger == null) Debug.LogError("HighlightTrigger component not found on this GameObject.");
    }

    // private void OnEnable()
    // {
    //     this.MMEventStartListening();
    // }
    //
    // private void OnDisable()
    // {
    //     this.MMEventStopListening();
    // }
    //
    // public void OnMMEvent(HighlightEvent eventType)
    // {
    //     // Early exit if event type is not what we're looking for
    //     if (eventType.EventType != HighlightEventType.ActivateTarget )
    //         return;
    //         
    //     // Early exit if this event is not targeted at this controller
    //     if (string.IsNullOrEmpty(targetID) || eventType.TargetID != targetID)
    //         return;
    //         
    //     ActivateTarget();
    // }

    // Simplified to not need a parameter since we already checked the targetID
    public void ActivateTarget()
    {
        if (highlightEffect != null) highlightEffect.targetFX = true;
    }

    public void DeactivateTarget()
    {
        if (highlightEffect != null) highlightEffect.targetFX = false;
    }
}