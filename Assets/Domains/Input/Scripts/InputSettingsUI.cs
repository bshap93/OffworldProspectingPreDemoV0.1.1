using UnityEngine;
using UnityEngine.UI;

namespace Domains.Input.Scripts
{
    public class InputSettingsUI : MonoBehaviour
    {
        [SerializeField] private Toggle invertYAxisToggle;
        [SerializeField] private Slider mouseSensitivitySlider;
        [SerializeField] private Slider controllerSensitivitySlider;

        private MyRewiredInputManager inputManager;

        private void OnEnable()
        {
            inputManager = MyRewiredInputManager.Instance;
            LoadSettings();
        }

        private void LoadSettings()
        {
            invertYAxisToggle.isOn = inputManager.InvertYAxis;
            mouseSensitivitySlider.value = inputManager.MouseSensitivity;
            controllerSensitivitySlider.value = inputManager.ControllerSensitivity;
        }

        public void OnInvertYAxisChanged(bool value)
        {
            inputManager.InvertYAxis = value;
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