using Domains.UI_Global.Briefings;
using Domains.UI_Global.Events;
using UnityEngine;

public class SignController : MonoBehaviour
{
    public bool hasAssociatedBriefing;
    public BriefingData briefingData;
    public int briefingIndex;


    public void TriggerBriefingUI()
    {
        if (hasAssociatedBriefing)
        {
            if (briefingData == null)
            {
                Debug.LogError("No briefing data associated with this sign.");
                return;
            }

            UIEvent.Trigger(UIEventType.OpenBriefing, briefingIndex);
        }
        else
        {
            Debug.LogError("No briefing data associated with this sign.");
        }
    }
}