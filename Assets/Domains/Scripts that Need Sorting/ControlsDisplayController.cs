using Domains.Input.Scripts;
using Domains.Player.Progression;
using UnityEngine;

public class ControlsDisplayController : MonoBehaviour
{
    private CanvasGroup canvasGroup;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) Debug.LogError("ControlsDisplayController: No CanvasGroup found on this GameObject.");
        ShowControls();
    }

    // Update is called once per frame
    private void Update()
    {
        if (CustomInputBindings.IsGetMoreInfoPressed())
            ShowControls();
        else if (ProgressionManager.TutorialFinished)
            HideControls();
    }

    private void ShowControls()
    {
        if (canvasGroup != null) canvasGroup.alpha = 1f;
    }

    private void HideControls()
    {
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }
}