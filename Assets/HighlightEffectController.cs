using HighlightPlus;
using UnityEngine;

public class HighlightEffectController : MonoBehaviour
{
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

    public void ActivateTarget()
    {
        if (highlightEffect != null) highlightEffect.targetFX = true;
    }

    public void DeactivateTarget()
    {
        if (highlightEffect != null) highlightEffect.targetFX = false;
    }
}