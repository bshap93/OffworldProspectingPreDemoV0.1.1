using Domains.UI_Global.Events;
using Domains.UI_Global.Interface;
using MoreMountains.Tools;
using NUnit.Framework;
using UnityEngine;

namespace Domains.UI_Global.Scripts
{
    public class UpgradeUIController : UIController 
    {
        private CanvasGroup _canvasGroup;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            Cursor.visible = false;        }

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
            if (eventType.EventType == UIEventType.OpenVendorConsole)
                OpenUI();
            else if (eventType.EventType == UIEventType.CloseVendorConsole) CloseUI();
            
        }


        public UpgradeUIController(bool isPaused) : base(isPaused)
        {
        }
    }
}