using Domains.Scene.Scripts;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ContinueGameButton : MonoBehaviour

{
    [SerializeField] private MMFeedbacks loadContinueGameFeedbacks;

    public void OnClick()
    {
        DiggerSaveUtility.Save(true, false);
        loadContinueGameFeedbacks?.PlayFeedbacks();
    }
}