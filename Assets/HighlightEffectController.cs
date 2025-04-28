using Domains.Effects.Highlighting;
using HighlightPlus;
using MoreMountains.Tools;
using UnityEngine;

public class HighlightEffectController : MonoBehaviour, MMEventListener<HighlightEvent>
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

    private void OnEnable()
    {
        this.MMEventStartListening();
    }

    private void OnDisable()
    {
        this.MMEventStopListening();
    }

    public void OnMMEvent(HighlightEvent eventType)
    {
        if (eventType.EventType == HighlightEventType.ActivateTarget) ActivateTarget();
    }


    public void ActivateTarget()
    {
        if (highlightEffect != null) highlightEffect.targetFX = true;
    }

    public void DeactivateTarget()
    {
        if (highlightEffect != null) highlightEffect.targetFX = false;
    }
}