using Domains.Scene.StaticScripts;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.Scene.MainMenu
{
    public class SceneLoadManager : MonoBehaviour, MMEventListener<MainMenuEvent>
    {
        private const string FilePath = "GameSave.es3";
        [SerializeField] private string sceneToLoad;


        private void OnEnable()
        {
            this.MMEventStartListening();
        }

        private void OnDisable()
        {
            this.MMEventStopListening();
        }

        public void OnMMEvent(MainMenuEvent eventType)
        {
            switch (eventType.EventType)
            {
                case MainMenuEventType.NewGameTriggered:
                    ClearAllSaveFilesOnly();
                    GameLoadFlags.IsNewGame = true;
                    LoadScene(sceneToLoad);
                    break;
                case MainMenuEventType.ContinueGameTriggered:
                    if (HasGameSave())
                    {
                        GameLoadFlags.IsNewGame = false; // Add this

                        LoadScene(sceneToLoad);
                    }


                    break;
                case MainMenuEventType.QuitGameTriggered:
                    Application.Quit();
                    break;
            }
        }

        public static void ClearAllSaveFilesOnly()
        {
            ES3.DeleteFile("GameSave.es3");
            ES3.DeleteFile("Pickables.es3");
            ES3.DeleteFile("Progression.es3");
            ES3.DeleteFile("UpgradeSave.es3");
        }

        public static bool HasGameSave()
        {
            return ES3.FileExists(FilePath);
        }

        public void LoadScene(string sceneName)
        {
            MMSceneLoadingManager.LoadScene(sceneName);
        }
    }
}