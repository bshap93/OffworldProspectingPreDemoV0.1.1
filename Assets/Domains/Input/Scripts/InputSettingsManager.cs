using Domains.Input.Events;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.Input.Scripts
{
    public class InputSettings : MonoBehaviour, MMEventListener<InputSettingsEvent>
    {
        [SerializeField] private bool invertYAxis;
        [SerializeField] private float mouseSensitivity = 1f;
        public static InputSettings Instance { get; private set; }


        public bool InvertYAxis
        {
            get => invertYAxis;
            set
            {
                invertYAxis = value;
                PlayerPrefs.SetInt("InvertYAxis", value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public float MouseSensitivity
        {
            get => PlayerPrefs.GetFloat("MouseSensitivity", 1f);
            set
            {
                PlayerPrefs.SetFloat("MouseSensitivity", value);
                PlayerPrefs.Save();
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // Load saved setting
                if (PlayerPrefs.HasKey("InvertYAxis"))
                    invertYAxis = PlayerPrefs.GetInt("InvertYAxis") == 1;

                if (PlayerPrefs.HasKey("MouseSensitivity"))
                    mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            this.MMEventStartListening();
        }

        private void OnDisable()
        {
            this.MMEventStopListening();
        }

        public void OnMMEvent(InputSettingsEvent eventType)
        {
            switch (eventType.EventType)
            {
                case InputSettingsEventType.InvertYAxis:
                    if (eventType.BoolValue != null) InvertYAxis = eventType.BoolValue.Value;
                    break;
                case InputSettingsEventType.SetMouseSensitivity:
                    if (eventType.FloatValue != null) MouseSensitivity = eventType.FloatValue.Value;
                    break;
            }
        }
    }
}