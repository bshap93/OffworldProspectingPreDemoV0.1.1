using System;
using System.Collections;
using Domains.Input.Scripts;
using Domains.Player.Events;
using Domains.Player.Progression;
using Domains.UI_Global.Events;
using HighlightPlus;
using INab.Dissolve;
using UnityEngine;

namespace Domains.Items.Scripts
{
    public class BioContainerInteractable : InteractableObjective
    {
        public GameObject interactablePrompt;
        [SerializeField] private Dissolver creatureDissolver;
        [SerializeField] private Light SpotLight;
        [SerializeField] private Color beforeColor;
        [SerializeField] private Color afterColor;


        private bool _interactionComplete;

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
            enabled = false;
        }

        public override void Interact()
        {
            if (hasBeenInteractedWith)
            {
                AlertEvent.Trigger(AlertReason.CreditsAdded, "Already scanned this BioContainer",
                    "BioContainer Scanned");
                return;
            }

            CurrencyEvent.Trigger(CurrencyEventType.AddCurrency, rewardAmount);
            AlertEvent.Trigger(AlertReason.CreditsAdded, $"{rewardAmount} Credits Added to your account",
                "BioContainer Scanned");

            interactFeedbacks?.PlayFeedbacks();
            hasBeenInteractedWith = true;
            ProgressionManager.AddInteractableObjective(uniqueID, true);
            OnInteractableInteract?.Invoke();
            StartCoroutine(DissolveCreatureThenDestroy());
        }


        private IEnumerator DissolveCreatureThenDestroy()
        {
            // Check if dissolver is assigned
            if (creatureDissolver == null)
            {
                UnityEngine.Debug.LogWarning("Dissolver not assigned! Destroying object without dissolve.");
                Destroy(creatureDissolver.gameObject);
                yield break;
            }

            // Start dissolving
            creatureDissolver.Dissolve();
            yield return new WaitForSeconds(creatureDissolver.duration);
            // Change light color
            if (SpotLight != null)
                SpotLight.color = afterColor;
            else
                UnityEngine.Debug.LogWarning("SpotLight not assigned! Cannot change light color.");
            // Destroy game object
            Destroy(creatureDissolver.gameObject);

            // Disable the HighlightEffect if it exists
            var highlightEffect = GetComponent<HighlightEffect>();
            if (highlightEffect != null)
                highlightEffect.enabled = false;

            // Disable the script
            var prompt = GetComponentInChildren<ButtonPromptWithAction>();
            Destroy(prompt);
        }


        private IEnumerator InitializeAfterProgressionManager()
        {
            // Wait a bit longer to ensure ProgressionManager has fully loaded
            yield return new WaitForSeconds(0.2f);

            if (ProgressionManager.IsObjectiveCollected(uniqueID))
            {
                UnityEngine.Debug.Log(
                    $"BioContainer {uniqueID} was previously collected. Initializing collected state.");

                hasBeenInteractedWith = true;
                if (interactablePrompt != null)
                    interactablePrompt.SetActive(false);

                if (SpotLight != null)
                    SpotLight.color = afterColor;

                // Add null check to prevent errors
                if (creatureDissolver != null)
                    Destroy(creatureDissolver.gameObject);
                else
                    UnityEngine.Debug.LogWarning($"BioContainer {uniqueID} has no creatureDissolver assigned");

                // Disable the HighlightEffect if it exists
                var highlightEffect = GetComponent<HighlightEffect>();
                if (highlightEffect != null)
                    highlightEffect.enabled = false;

                // Disable the script
                var prompt = GetComponentInChildren<ButtonPromptWithAction>();
                Destroy(prompt);
            }
        }
    }
}