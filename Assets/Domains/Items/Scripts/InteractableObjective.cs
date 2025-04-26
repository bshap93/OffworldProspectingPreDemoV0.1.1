using CompassNavigatorPro;
using Domains.Gameplay.Mining.Scripts;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Domains.Items.Scripts
{
    public abstract class InteractableObjective : MonoBehaviour, IInteractable
    {
        [FormerlySerializedAs("UniqueID")] public string uniqueID;

        [FormerlySerializedAs("RewardAmount")] public int rewardAmount;

        [SerializeField] protected bool hasBeenInteractedWith;
        [Header("Events")] public UnityEvent OnInteractableInteract;


        public MMFeedbacks interactFeedbacks;


        protected CompassPro compassPro;

        protected CompassProPOI compassProPOI;

        protected virtual void Start()
        {
            compassProPOI = GetComponent<CompassProPOI>();
            if (compassProPOI != null)
            {
                compassProPOI.ToggleIndicatorVisibility(false);


                UnityEngine.Debug.Log("POI visibility set to always hidden");
            }


            compassPro = FindFirstObjectByType<CompassPro>();
            if (compassPro != null) compassPro.UpdateSettings();
        }

        public abstract void Interact();

        public void ShowInteractablePrompt()
        {
        }

        public void HideInteractablePrompt()
        {
            if (compassProPOI != null) compassProPOI.ToggleIndicatorVisibility(false);
        }
    }
}