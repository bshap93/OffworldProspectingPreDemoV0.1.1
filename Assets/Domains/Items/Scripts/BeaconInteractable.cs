using System;
using System.Collections;
using Domains.Player.Events;
using Domains.Player.Progression;
using Domains.UI_Global.Events;
using INab.Dissolve;
using UnityEngine;
using UnityEngine.Serialization;

namespace Domains.Items.Scripts
{
    public class BeaconInteractable : InteractableObjective
    {
        public GameObject interactablePrompt;

        [SerializeField] private Dissolver dissolver;

        [FormerlySerializedAs("IsObjective")] public bool isObjective;


        private bool _interactionComplete;
        private bool _isBeingDestroyed;
        private bool _isInRange;

        private void Awake()
        {
            if (string.IsNullOrEmpty(uniqueID)) uniqueID = Guid.NewGuid().ToString(); // Generate only if unset
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(InitializeAfterProgressionManager());

            if (interactFeedbacks != null) interactFeedbacks.Initialization();
        }

        private void OnDestroy()
        {
            _isBeingDestroyed = true;

            _isInRange = false;
            enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) _isInRange = true;
        }

        private void OnTriggerExit(Collider exitCollider)
        {
            if (exitCollider.CompareTag("Player")) _isInRange = false;
        }

        private IEnumerator InitializeAfterProgressionManager()
        {
            yield return null;


            if (ProgressionManager.IsObjectiveCollected(uniqueID)) Destroy(gameObject);
        }

        public override void Interact()
        {
            if (hasBeenInteractedWith) return;
            CurrencyEvent.Trigger(CurrencyEventType.AddCurrency, rewardAmount);
            AlertEvent.Trigger(AlertReason.CreditsAdded, rewardAmount! + " Credits Added to your account",
                "Beacon Interacted", null, null, Color.white);

            interactFeedbacks?.PlayFeedbacks();
            hasBeenInteractedWith = true;
            ProgressionManager.AddInteractableObjective(uniqueID, true);
            OnInteractableInteract?.Invoke();
            StartCoroutine(DissolveThenDestroy());
        }

        public void TryCompleteObjective()
        {
            if (isObjective)
            {
            }
        }

        private IEnumerator DissolveThenDestroy()
        {
            // Check if dissolver is assigned
            if (dissolver == null)
            {
                UnityEngine.Debug.LogWarning("Dissolver not assigned! Destroying object without dissolve.");
                Destroy(gameObject);
                yield break;
            }

            // Start dissolving
            dissolver.Dissolve();
            yield return new WaitForSeconds(dissolver.duration);
            // Destroy game object
            Destroy(gameObject);
        }
    }
}