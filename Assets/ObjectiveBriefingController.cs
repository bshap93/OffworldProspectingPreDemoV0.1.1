using Domains.UI_Global.Briefings;
using UnityEngine;

public class ObjectiveBriefingController : MonoBehaviour
{
    [SerializeField] private BriefingData[] objectiveBriefing;


    public void ShowObjectiveBriefing(int briefingIndex)
    {
        if (briefingIndex < 0 || briefingIndex >= objectiveBriefing.Length)
        {
            Debug.LogError("Invalid briefing index.");
            return;
        }

        var briefing = objectiveBriefing[briefingIndex];
        // Display the briefing data in your UI
        // For example, you can set the header image, text, and description in your UI elements
    }

    public void HideObjectiveBriefing()
    {
        // Hide the briefing UI
    }
}