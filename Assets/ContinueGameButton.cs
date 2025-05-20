using Domains.Scene.Scripts;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ContinueGameButton : MonoBehaviour

{
    [SerializeField] private MMFeedbacks loadContinueGameFeedbacks;

    private void Awake()
    {
        if (!ES3.FileExists("GameSave.es3"))
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public void OnClick()
    {
        DiggerSaveUtility.Save(true, false);
        loadContinueGameFeedbacks?.PlayFeedbacks();
    }
}