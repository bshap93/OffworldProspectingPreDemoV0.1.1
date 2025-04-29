using UnityEngine;

namespace Domains.UI_Global.Scripts
{
    public class InfoDumpData : ScriptableObject
    {
        // Consider upgrading to a GIF
        public Sprite headerImage;
        public string headerText;

        public string firstParagraph;
        public string secondParagraph;
        public string thirdParagraph;

        public string InfoId;
    }
}