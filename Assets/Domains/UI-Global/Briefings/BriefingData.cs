using PixelCrushers.QuestMachine;
using UnityEngine;

namespace Domains.UI_Global.Briefings
{
    [CreateAssetMenu(fileName = "BriefingData", menuName = "Objectives/BriefingData")]
    public class BriefingData : ScriptableObject
    {
        public string briefingId;

        // Consider upgrading to a GIF
        public Sprite headerImage;
        public string headerText;

        public string descriptionText;

        public Quest quest;


        public Sprite objectiveSprite;
    }
}