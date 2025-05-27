using Domains.Debug;
using MoreMountains.Feedbacks;
using PixelCrushers;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] private MMFeedbacks loadNewGameFeedbacks;
    [SerializeField] AutoSaveLoad autoSaveLoad;


    public void OnClick()
    {
        DataReset.ClearAllSaveData();




        loadNewGameFeedbacks?.PlayFeedbacks();
    }
}