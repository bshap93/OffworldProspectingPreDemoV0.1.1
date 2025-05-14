using Domains.UI_Global.Events;
using MoreMountains.Feedbacks;
using UnityEngine;

public class CommsComputer : MonoBehaviour
{
    [SerializeField] private MMFeedbacks openCommsComputerFB;


    public void OpenCommsComputer()
    {
        UIEvent.Trigger(UIEventType.OpenCommsComputer);
        openCommsComputerFB?.PlayFeedbacks();
    }
}