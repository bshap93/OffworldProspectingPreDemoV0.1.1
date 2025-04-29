using PixelCrushers.QuestMachine;
using UnityEngine;

public class ObjectiveTile : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private string headerText;

    [SerializeField] private string headerImage;


    [SerializeField] private string descriptionText;

    [Header("Quest Data")] [SerializeField]
    private QuestNode questNode;

    public void SetObjectiveTile(QuestNode questNode)
    {
        this.questNode = questNode;
        if (!string.IsNullOrEmpty(questNode.internalName.text))
            headerText = questNode.internalName.text;
        else
            headerText = questNode.id.text;
    }
}