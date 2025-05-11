using UnityEngine;
using UnityEngine.SceneManagement;

namespace Domains.Input.Scripts
{
    public class MainMenuManager : MonoBehaviour
    {
        private MyRewiredInputManager inputManager;
        private string mainMenuName = "";
        public static MainMenuManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                mainMenuName = SceneManager.GetActiveScene().name;
            }
            else
            {
                Destroy(gameObject);
            }

            inputManager = MyRewiredInputManager.Instance;

            if (inputManager == null)
                UnityEngine.Debug.LogError("MainMenuManager: No MyRewiredInputManager found in scene!");
        }


        private void Update()
        {
            if (inputManager.IsPausePressed())
            {
                if (SceneManager.GetActiveScene().name == mainMenuName)
                    Application.Quit();
                else
                    GoToScene(mainMenuName);
            }
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void GoToScene(string sceneName)
        {
            if (sceneName == mainMenuName)
                Cursor.visible = true;
            else
                Cursor.visible = false;

            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}