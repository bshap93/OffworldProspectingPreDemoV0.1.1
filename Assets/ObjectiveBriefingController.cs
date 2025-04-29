using Domains.UI_Global.Briefings;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectiveBriefingController : MonoBehaviour
{
    [SerializeField] private BriefingData[] objectiveBriefing;

    [Header("Header")] [SerializeField] private TMP_Text headerText;

    [SerializeField] private TMP_Text descriptionText;

    [Header("Objective Tiles")] [SerializeField]
    private ObjectiveTile objectiveTile01;

    [SerializeField] private ObjectiveTile objectiveTile02;
    [SerializeField] private ObjectiveTile objectiveTile03;
    [SerializeField] private ObjectiveTile objectiveTile04;
    [SerializeField] private Image headerImage;


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