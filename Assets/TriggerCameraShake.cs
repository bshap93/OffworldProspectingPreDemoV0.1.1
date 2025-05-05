using Domains.UI_Global.Events;
using UnityEngine;

public class TriggerCameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.5f;

    public void TriggerShake()
    {
        CameraEvent.Trigger(CameraEventType.CameraShake, shakeDuration, shakeMagnitude);
    }
}