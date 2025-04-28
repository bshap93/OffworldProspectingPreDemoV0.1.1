using Domains.UI_Global.Events;
using Domains.UI_Global.Interface;
using UnityEngine;

namespace Domains.UI_Global.Scripts
{
    public class QuestDialogueUIController : UIController
    {
        private CanvasGroup _canvasGroup;


        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            CloseUI();
        }

        public override void CloseUI()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            IsPaused = false;
            // Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }

        public override void OpenUI()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            IsPaused = true;
            // Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void OnMMEvent(UIEvent eventType)
        {
            if (eventType.EventType == UIEventType.OpenQuestDialogue)
                OpenUI();
            else if (eventType.EventType == UIEventType.CloseQuestDialogue) CloseUI();
        }
        public QuestDialogueUIController(bool isPaused) : base(isPaused)
        {
        }
    }
}