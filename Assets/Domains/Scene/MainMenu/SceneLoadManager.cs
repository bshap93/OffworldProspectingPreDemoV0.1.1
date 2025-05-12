using System;
using System.Collections;
using System.IO;
using Domains.Scene.StaticScripts;
using MoreMountains.Tools;
using ThirdParty.Feel.MMTools.Core.MMSceneLoading.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Domains.Scene.MainMenu
{
    public class SceneLoadManager : MonoBehaviour, MMEventListener<MainMenuEvent>
    {
        private const string FilePath = "GameSave.es3";
        [SerializeField] public string sceneToLoad;
        [SerializeField] public string loadingScreenName = "LoadingScreen";

        // Track if we've already triggered a scene load
        private bool _loadingInProgress;

        private void Awake()
        {
            // Force initialize LoadingScreenSceneName at the earliest point
            MMSceneLoadingManager.LoadingScreenSceneName = loadingScreenName;
            UnityEngine.Debug.Log(
                $"[SceneLoadManager] Initialized LoadingScreenSceneName to: {MMSceneLoadingManager.LoadingScreenSceneName}");

            // Verify build settings
            VerifySceneInBuildSettings(sceneToLoad);
            VerifySceneInBuildSettings(loadingScreenName);

            // Set consistent thread priority for loading
            Application.backgroundLoadingPriority = ThreadPriority.High;
        }

        private void Start()
        {
            UnityEngine.Debug.Log($"[SceneLoadManager] SceneToLoad: {sceneToLoad}, HasSave: {HasGameSave()}");

            // Make sure loading screen name is still set
            if (string.IsNullOrEmpty(MMSceneLoadingManager.LoadingScreenSceneName))
            {
                UnityEngine.Debug.LogWarning("[SceneLoadManager] LoadingScreenSceneName was reset, reinitializing");
                MMSceneLoadingManager.LoadingScreenSceneName = loadingScreenName;
            }
        }

        private void OnEnable()
        {
            UnityEngine.Debug.Log("[SceneLoadManager] Registering event listener");
            this.MMEventStartListening();
        }

        private void OnDisable()
        {
            UnityEngine.Debug.Log("[SceneLoadManager] Unregistering event listener");
            this.MMEventStopListening();
        }

        public void OnMMEvent(MainMenuEvent eventType)
        {
            UnityEngine.Debug.Log($"[SceneLoadManager] Received event: {eventType.EventType}");

            // Prevent multiple loads from being triggered
            if (_loadingInProgress)
            {
                UnityEngine.Debug.Log("[SceneLoadManager] Loading already in progress, ignoring event");
                return;
            }

            switch (eventType.EventType)
            {
                case MainMenuEventType.NewGameTriggered:
                    UnityEngine.Debug.Log("[SceneLoadManager] Processing NewGameTriggered");
                    _loadingInProgress = true;
                    ClearAllSaveFilesOnly();
                    GameLoadFlags.IsNewGame = true;
                    // LoadScene(sceneToLoad);
                    // Use MMSceneLoadingManager for loading
                    MMSceneLoadingManager.LoadScene(sceneToLoad, loadingScreenName);
                    break;

                case MainMenuEventType.ContinueGameTriggered:
                    UnityEngine.Debug.Log("[SceneLoadManager] Processing ContinueGameTriggered");
                    if (HasGameSave())
                    {
                        _loadingInProgress = true;
                        UnityEngine.Debug.Log($"[SceneLoadManager] Save file exists, loading scene: {sceneToLoad}");
                        GameLoadFlags.IsNewGame = false;

                        // Double-check that LoadingScreenSceneName is set
                        if (string.IsNullOrEmpty(MMSceneLoadingManager.LoadingScreenSceneName))
                        {
                            MMSceneLoadingManager.LoadingScreenSceneName = loadingScreenName;
                            UnityEngine.Debug.Log(
                                $"[SceneLoadManager] Reset LoadingScreenSceneName to {loadingScreenName}");
                        }

                        // LoadScene(sceneToLoad);
                        // Use MMSceneLoadingManager for loading
                        MMSceneLoadingManager.LoadScene(sceneToLoad, loadingScreenName);
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning(
                            "[SceneLoadManager] Continue game triggered but no save file exists!");
                    }

                    break;

                case MainMenuEventType.QuitGameTriggered:
                    UnityEngine.Debug.Log("[SceneLoadManager] Processing QuitGameTriggered");
                    Application.Quit();
                    break;
            }
        }

        public static void ClearAllSaveFilesOnly()
        {
            try
            {
                UnityEngine.Debug.Log("[SceneLoadManager] Clearing all save files");
                if (ES3.FileExists("GameSave.es3")) ES3.DeleteFile("GameSave.es3");
                if (ES3.FileExists("Pickables.es3")) ES3.DeleteFile("Pickables.es3");
                if (ES3.FileExists("Progression.es3")) ES3.DeleteFile("Progression.es3");
                if (ES3.FileExists("UpgradeSave.es3")) ES3.DeleteFile("UpgradeSave.es3");
                UnityEngine.Debug.Log("[SceneLoadManager] All save files cleared successfully");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[SceneLoadManager] Error clearing save files: {e.Message}");
            }
        }

        public static bool HasGameSave()
        {
            try
            {
                var exists = ES3.FileExists(FilePath);
                UnityEngine.Debug.Log($"[SceneLoadManager] Checking for save file at default path: {exists}");

                // If not found at default path, try with explicit path
                if (!exists)
                {
                    var fullPath = Path.Combine(Application.persistentDataPath, FilePath);
                    exists = File.Exists(fullPath);
                    UnityEngine.Debug.Log(
                        $"[SceneLoadManager] Checking for save file at full path {fullPath}: {exists}");
                }

                return exists;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[SceneLoadManager] Error checking for save file: {e.Message}");
                return false;
            }
        }

        public void LoadScene(string sceneName)
        {
            try
            {
                UnityEngine.Debug.Log($"[SceneLoadManager] Loading scene: {sceneName} via MMSceneLoadingManager");

                // Ensure loading screen name is set one last time
                MMSceneLoadingManager.LoadingScreenSceneName = loadingScreenName;

                // Try MM loading first
                MMSceneLoadingManager.LoadScene(sceneName, loadingScreenName);

                // Start emergency timer as backup
                StartCoroutine(EmergencyLoadTimer(sceneName, 10f));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(
                    $"[SceneLoadManager] Error in MMSceneLoadingManager: {e.Message}, attempting direct load");
                // Fall back to direct loading if MM loading fails
                LoadSceneDirect(sceneName);
            }
        }

        public void LoadSceneDirect(string sceneName)
        {
            try
            {
                UnityEngine.Debug.Log($"[SceneLoadManager] DIRECT LOADING scene: {sceneName}");
                SceneManager.LoadScene(sceneName);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[SceneLoadManager] CRITICAL: Direct scene load failed: {e.Message}");
                // Display error to user if possible
                ShowErrorMessage("Failed to load scene. Please restart the game.");
            }
        }

        private IEnumerator EmergencyLoadTimer(string sceneName, float timeout)
        {
            UnityEngine.Debug.Log($"[SceneLoadManager] Starting emergency timer for {timeout} seconds");
            yield return new WaitForSeconds(timeout);

            // If we're still in the same scene after timeout
            if (SceneManager.GetActiveScene().name != sceneName &&
                SceneManager.GetActiveScene().name != loadingScreenName)
            {
                UnityEngine.Debug.LogWarning(
                    "[SceneLoadManager] EMERGENCY: Loading timeout exceeded, attempting direct load");
                LoadSceneDirect(sceneName);
            }
        }

        private void VerifySceneInBuildSettings(string sceneName)
        {
            var sceneInBuild = false;

            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePathInBuild = SceneUtility.GetScenePathByBuildIndex(i);
                var sceneNameInBuild = Path.GetFileNameWithoutExtension(scenePathInBuild);

                if (sceneNameInBuild == sceneName)
                {
                    sceneInBuild = true;
                    UnityEngine.Debug.Log(
                        $"[SceneLoadManager] Verified scene in build settings: {sceneName} at index {i}");
                    break;
                }
            }

            if (!sceneInBuild)
                UnityEngine.Debug.LogError(
                    $"[SceneLoadManager] CRITICAL: Scene '{sceneName}' is NOT in build settings!");
        }

        private void ShowErrorMessage(string message)
        {
            UnityEngine.Debug.LogError($"[SceneLoadManager] CRITICAL ERROR: {message}");
            // In a real implementation, you'd show UI here
        }
    }
}