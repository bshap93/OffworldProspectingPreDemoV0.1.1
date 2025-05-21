using System.Collections;
using Domains.Gameplay.Equipment.Events;
using Domains.Input.Scripts;
using Domains.Player.Progression;
using Domains.Scripts_that_Need_Sorting;
using Domains.UI_Global.Events;
using UnityEngine;

namespace Domains.Items.Scripts
{
    public class EquipmentPickupInteractable : InteractableObjective
    {
        private InfoPanelActivator _infoPanelActivator;

        protected override void Start()
        {
            base.Start();
            _infoPanelActivator = GetComponent<InfoPanelActivator>();
            if (interactFeedbacks != null) interactFeedbacks.Initialization();
        }

        protected override IEnumerator InitializeAfterProgressionManager()
        {
            yield return null;


            if (ProgressionManager.IsObjectiveCollected(uniqueID)) Destroy(gameObject);
        }

        public override void Interact()
        {
            if (hasBeenInteractedWith) return;
            if (_infoPanelActivator != null) _infoPanelActivator.ToggleInfoPanel();
        }

        public void PickupEquipment()
        {
            EquipmentEvent.Trigger(EquipmentEventType.PickupEquipment, ToolType.Jetpack);
            AlertEvent.Trigger(AlertReason.PickedUpEquipment,
                "Picked up Jetpack", "Jetpack Picked Up", null, null, Color.white);
        }
    }
}