using CompassNavigatorPro;
using Domains.Gameplay.Equipment.Events;
using Domains.Player.Events;
using Domains.Scripts_that_Need_Sorting;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.Scene.Location
{
    public class CompassProController : MonoBehaviour, MMEventListener<EquipmentEvent>,
        MMEventListener<ProgressionEvent>
    {
        [SerializeField] private CompassPro compassPro;
        private bool isScannerEquipped;
        private bool isTutorialFinished;

        private void Start()
        {
            if (compassPro == null) compassPro = FindFirstObjectByType<CompassPro>();

            if (compassPro == null)
            {
                UnityEngine.Debug.LogError("CompassPro component not found on this GameObject.");
                return;
            }

            compassPro.showOnScreenIndicators = false;
            compassPro.UpdateSettings();
        }

        private void OnEnable()
        {
            this.MMEventStartListening<EquipmentEvent>();
            this.MMEventStartListening<ProgressionEvent>();
        }

        private void OnDisable()
        {
            this.MMEventStopListening<EquipmentEvent>();
            this.MMEventStopListening<ProgressionEvent>();
        }

        public void OnMMEvent(EquipmentEvent eventType)
        {
            if (eventType.ToolType == ToolType.Scanner)
            {
                if (compassPro != null)
                {
                    compassPro.showOnScreenIndicators = true;
                    isScannerEquipped = true;
                    compassPro.UpdateSettings();
                }
            }
            else if (eventType.ToolType == ToolType.Pickaxe || eventType.ToolType == ToolType.Shovel)
            {
                if (compassPro != null)
                {
                    compassPro.showOnScreenIndicators = false;
                    isScannerEquipped = false;
                    compassPro.UpdateSettings();
                }
            }
        }

        public void OnMMEvent(ProgressionEvent eventType)
        {
            if (eventType.EventType == ProgressionEventType.StartTutorial)
            {
                compassPro.showOnScreenIndicators = true;
                compassPro.UpdateSettings();
            }
            else if (eventType.EventType == ProgressionEventType.FinishTutorial)
            {
                if (!isScannerEquipped)
                {
                    compassPro.showOnScreenIndicators = false;
                    compassPro.UpdateSettings();
                }
            }
        }
    }
}