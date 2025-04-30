using Domains.Gameplay.Objectives.Scripts;
using PixelCrushers.QuestMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTile : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private TMP_Text headerText;

    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private Image headerImage;


    public void SetObjectiveTile(QuestNode questNode)
    {
        if (!string.IsNullOrEmpty(questNode.internalName.text))
            headerText.text = questNode.internalName.text;
        else
            headerText.text = questNode.id.text;

        var content = questNode.GetStateInfo(QuestNodeState.Active);


        if (content != null)
        {
            var activeDialogueText = content.categorizedContentList[(int)QuestContentCategory.Dialogue].ToString();
            if (!string.IsNullOrEmpty(activeDialogueText))
                descriptionText.text = activeDialogueText;
            else
                descriptionText.text = "No content available.";
        }
        else
        {
            descriptionText.text = "No content available.";
        }

        if (ObjectiveManager.Instance.nullObjectiveImage != null)
        {
            headerImage.sprite = ObjectiveManager.Instance.nullObjectiveImage;
        }
        else
        {
            headerImage.sprite = null;
            Debug.LogError("Null Objective Image not set in ObjectiveManager.");
        }
    }

    public void EmptyObjectiveTile()
    {
        headerText.text = string.Empty;
        descriptionText.text = string.Empty;
        if (ObjectiveManager.Instance.nullObjectiveImage != null)
        {
            headerImage.sprite = ObjectiveManager.Instance.nullObjectiveImage;
        }
        else
        {
            headerImage.sprite = null;
            Debug.LogError("Null Objective Image not set in ObjectiveManager.");
        }
    }
}