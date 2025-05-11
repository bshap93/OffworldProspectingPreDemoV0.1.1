using Domains.Input.Events;
using Michsky.UI.Shift;
using UnityEngine;

namespace Domains.Input.Scripts
{
    public class InputSettingsUI : MonoBehaviour
    {
        [SerializeField] private SwitchManager invertYAxisToggle;

        private void OnEnable()
        {
            invertYAxisToggle.isOn = InputSettings.Instance.InvertYAxis;
        }

        public void OnInvertYAxisChanged(bool value)
        {
            InputSettingsEvent.Trigger(InputSettingsEventType.InvertYAxis, value);
        }
    }
}