using Domains.UI_Global.Briefings;
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
        }
        else
        {
            Debug.LogError("No briefing data associated with this sign.");
        }
    }
}