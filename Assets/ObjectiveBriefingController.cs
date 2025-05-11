using Domains.Input.Scripts;
using Domains.UI_Global.Briefings;
using Domains.UI_Global.Events;
using MoreMountains.Tools;
using PixelCrushers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ObjectiveBriefingController : MonoBehaviour, MMEventListener<UIEvent>
{
    [FormerlySerializedAs("objectiveBriefing")] [SerializeField]
    private BriefingData[] objectiveBriefings;

    [Header("Header")] [SerializeField] private TMP_Text headerText;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private TMP_Text descriptionText;

    [Header("Objective Tiles")] [SerializeField]
    private ObjectiveTile objectiveTile01;

    [SerializeField] private ObjectiveTile objectiveTile02;
    [SerializeField] private ObjectiveTile objectiveTile03;
    [SerializeField] private ObjectiveTile objectiveTile04;
    public Image HeaderImage;
    [SerializeField] private UnityEvent onCloseButtonClicked;


    private BriefingData currentBriefing;

    private MyRewiredInputManager inputManager;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            Debug.LogError("ObjectiveBriefingController: No CanvasGroup found on this GameObject.");

        inputManager = MyRewiredInputManager.Instance;
        if (inputManager == null)
            Debug.LogError("ObjectiveBriefingController: No MyRewiredInputManager found in the scene.");
    }

    private void Update()
    {
        if (inputManager.IsPausePressed())
        {
            HideObjectiveBriefing();
            onCloseButtonClicked?.Invoke();
            // Close the UI
            UIEvent.Trigger(UIEventType.CloseBriefing);
        }
    }

    private void OnEnable()
    {
        this.MMEventStartListening();
    }

    private void OnDisable()
    {
        this.MMEventStopListening();
    }

    public void OnMMEvent(UIEvent eventType)
    {
        if (eventType.EventType == UIEventType.OpenBriefing)
            ShowObjectiveBriefing(eventType.Index);
        else if (eventType.EventType == UIEventType.CloseBriefing) HideObjectiveBriefing();

        HideObjectiveBriefing();
    }

    public void ShowObjectiveBriefing(int briefingIndex)
    {
        if (briefingIndex < 0 || briefingIndex >= objectiveBriefings.Length)
        {
            Debug.LogError("Invalid briefing index.");
            return;
        }

        currentBriefing = objectiveBriefings[briefingIndex];
        // Display the briefing data in your UI
        // For example, you can set the header image, text, and description in your UI elements
        HeaderImage.sprite = currentBriefing.headerImage;
        headerText.text = currentBriefing.headerText;
        descriptionText.text = currentBriefing.descriptionText;
        var questNodes = currentBriefing.quest.nodeList;
        if (questNodes.Count > 0)
            objectiveTile01.SetObjectiveTile(questNodes[0]);
        else
            objectiveTile01.EmptyObjectiveTile();
        if (questNodes.Count > 1)
            objectiveTile02.SetObjectiveTile(questNodes[1]);
        else
            objectiveTile02.EmptyObjectiveTile();
        if (questNodes.Count > 2)
            objectiveTile03.SetObjectiveTile(questNodes[2]);
        else
            objectiveTile03.EmptyObjectiveTile();
        if (questNodes.Count > 3)
            objectiveTile04.SetObjectiveTile(questNodes[3]);
        else
            objectiveTile04.EmptyObjectiveTile();

        // Make the briefing UI visible
        MakeBriefingVisible(briefingIndex);
    }

    public void HideObjectiveBriefing()
    {
        // Hide the briefing UI

        headerText.text = string.Empty;
        descriptionText.text = string.Empty;
        HeaderImage.sprite = null;
        objectiveTile01.EmptyObjectiveTile();
        objectiveTile02.EmptyObjectiveTile();
        objectiveTile03.EmptyObjectiveTile();
        objectiveTile04.EmptyObjectiveTile();

        if (currentBriefing != null)
            MessageSystem.SendMessage(this, "Briefed", currentBriefing.briefingId);

        MakeBriefingInvisible();
    }


    private void MakeBriefingVisible(int index)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            // Time.timeScale = 0;
            // UIEvent.Trigger(UIEventType.OpenBriefing, index);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("InfoDumpController: No CanvasGroup found on this GameObject.");
        }
    }

    private void MakeBriefingInvisible()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            // Time.timeScale = 1;
            // UIEvent.Trigger(UIEventType.CloseBriefing);


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}