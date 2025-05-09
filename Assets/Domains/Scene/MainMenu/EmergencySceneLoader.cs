using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domains.Scene.MainMenu
{
    public class EmergencySceneLoader : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad = "MainScene08";
        [SerializeField] private float autoLoadTimeout = 30f; // Loads scene automatically after this many seconds
        [SerializeField] private Button emergencyButton;
        [SerializeField] private bool hideButtonNormally = true;

        private void Start()
        {
            // Auto-find the scene to load from SceneLoadManager if available
            var sceneManager = FindObjectOfType<SceneLoadManager>();
            if (sceneManager != null && !string.IsNullOrEmpty(sceneManager.sceneToLoad))
            {
                sceneToLoad = sceneManager.sceneToLoad;
                UnityEngine.Debug.Log($"[EmergencyLoader] Found scene to load: {sceneToLoad}");
            }

            // Set up button
            if (emergencyButton == null) emergencyButton = GetComponent<Button>();

            if (emergencyButton != null)
            {
                // Hide the button by default if requested
                if (hideButtonNormally)
                {
                    emergencyButton.gameObject.SetActive(false);
                    StartCoroutine(ShowButtonAfterDelay(15f)); // Show after 15 seconds
                }

                emergencyButton.onClick.AddListener(LoadSceneDirectly);
            }

            // Start auto-load timer if enabled
            if (autoLoadTimeout > 0) StartCoroutine(AutoLoadAfterTimeout());
        }

        public void LoadSceneDirectly()
        {
            UnityEngine.Debug.Log($"[EmergencyLoader] EMERGENCY: Direct loading scene {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }

        private IEnumerator ShowButtonAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            // If still in main menu, show the emergency button
            if (emergencyButton != null && !emergencyButton.gameObject.activeSelf)
            {
                UnityEngine.Debug.Log("[EmergencyLoader] Showing emergency button after timeout");
                emergencyButton.gameObject.SetActive(true);
            }
        }

        private IEnumerator AutoLoadAfterTimeout()
        {
            yield return new WaitForSeconds(autoLoadTimeout);

            // Auto-load the scene as a last resort
            UnityEngine.Debug.LogWarning("[EmergencyLoader] Auto-loading scene after timeout");
            LoadSceneDirectly();
        }
    }
}