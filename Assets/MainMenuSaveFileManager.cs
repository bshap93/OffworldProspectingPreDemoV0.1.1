using Domains.Scene.Scripts;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuSaveFileManager : MonoBehaviour
{
    [SerializeField] private GameObject continueGameButton;
    private bool hasGameSavedSinceNewGame;


    private void Start()
    {
        hasGameSavedSinceNewGame =
            ES3.Load<bool>(SaveManager.HasGameSavedSinceNewGame, SaveManager.SaveFileName);

        if (!hasGameSavedSinceNewGame)
        {
            continueGameButton.gameObject.GetComponent<Button>().SetEnabled(false);
            Debug.Log("Continue Game Button Disabled");
        }
    }
}