using Domains.Input.Events;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.Input.Scripts
{
    public class InputSettings : MonoBehaviour, MMEventListener<InputSettingsEvent>
    {
        [SerializeField] private bool invertYAxis;
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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // Load saved setting
                if (PlayerPrefs.HasKey("InvertYAxis"))
                    invertYAxis = PlayerPrefs.GetInt("InvertYAxis") == 1;
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
            }
        }
    }
}