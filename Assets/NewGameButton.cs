using Domains.Debug;
using MoreMountains.Feedbacks;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] private MMFeedbacks loadNewGameFeedbacks;


    public void OnClick()
    {
        DataReset.ClearAllSaveData();

        PlayerPrefs.SetInt("IsNewGame", 1);

        loadNewGameFeedbacks?.PlayFeedbacks();
    }
}