using Domains.Input.Events;
using Michsky.UI.Shift;
using UnityEngine;

namespace Domains.Input.Scripts
{
    public class InputSettingsUI : MonoBehaviour
    {
        [SerializeField] private SwitchManager invertYAxisToggle;
        [SerializeField] private SliderManager mouseSensitivitySlider;
        [SerializeField] private SliderManager controllerSensitivitySlider;

        private MyRewiredInputManager inputManager;

        private void OnEnable()
        {
            inputManager = MyRewiredInputManager.Instance;
            LoadSettings();
        }

        private void LoadSettings()
        {
            invertYAxisToggle.isOn = inputManager.InvertYAxis;
            mouseSensitivitySlider.mainSlider.value = inputManager.MouseSensitivity;
            controllerSensitivitySlider.mainSlider.value = inputManager.ControllerSensitivity;
        }

        public void OnInvertYAxisChanged(bool value)
        {
            inputManager.InvertYAxis = value;
            InputSettingsEvent.Trigger(InputSettingsEventType.InvertYAxis, value);
        }

        public void OnMouseSensitivityChanged(float value)
        {
            inputManager.MouseSensitivity = value;
        }

        public void OnControllerSensitivityChanged(float value)
        {
            inputManager.ControllerSensitivity = value;
        }
    }
}