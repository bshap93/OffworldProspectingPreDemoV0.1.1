using Domains.Input.Events;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

namespace Domains.Input.Scripts
{
    [DefaultExecutionOrder(-100)] // Execute early in the update cycle
    public class MyRewiredInputManager : MonoBehaviour, MMEventListener<InputSettingsEvent>
    {
        private static MyRewiredInputManager _instance;
        [SerializeField] private int playerCount = 1;

        // Settings for configuration
        [Header("Input Settings")] [SerializeField]
        private bool invertYAxis;

        [SerializeField] private float mouseSensitivity = 1.0f;
        [SerializeField] private float controllerSensitivity = 1.0f;

        private Rewired.Player[] players;

        public static MyRewiredInputManager Instance
        {
            get
            {
                if (_instance == null)
                    UnityEngine.Debug.LogError("RewiredInputManager not found in scene!");
                return _instance;
            }
        }

        // Getters/setters for settings
        public bool InvertYAxis
        {
            get => invertYAxis;
            set
            {
                invertYAxis = value;
                SaveInputSettings();
            }
        }

        public float MouseSensitivity
        {
            get => mouseSensitivity;
            set
            {
                mouseSensitivity = value;
                SaveInputSettings();
            }
        }

        public float ControllerSensitivity
        {
            get => controllerSensitivity;
            set
            {
                controllerSensitivity = value;
                SaveInputSettings();
            }
        }

        private void Awake()
        {
            // Singleton pattern setup
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize players array
            players = new Rewired.Player[playerCount];
            for (var i = 0; i < playerCount; i++) players[i] = ReInput.players.GetPlayer(i);

            // Load user preferences if saved
            LoadInputSettings();
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
                    InvertYAxis = eventType.BoolValue ?? InvertYAxis;
                    break;
            }
        }

        private void LoadInputSettings()
        {
            // Load from PlayerPrefs or other save system
            if (PlayerPrefs.HasKey("InvertYAxis"))
                invertYAxis = PlayerPrefs.GetInt("InvertYAxis") == 1;

            if (PlayerPrefs.HasKey("MouseSensitivity"))
                mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");

            if (PlayerPrefs.HasKey("ControllerSensitivity"))
                controllerSensitivity = PlayerPrefs.GetFloat("ControllerSensitivity");
        }

        public void SaveInputSettings()
        {
            PlayerPrefs.SetInt("InvertYAxis", invertYAxis ? 1 : 0);
            PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
            PlayerPrefs.SetFloat("ControllerSensitivity", controllerSensitivity);
            PlayerPrefs.Save();
        }

        // Input methods similar to your existing CustomInputBindings
        public bool IsInteractPressed(int playerId = 0)
        {
            return players[playerId].GetButtonDown("Interact");
        }

        public bool IsGetMoreInfoPressed(int playerId = 0)
        {
            return players[playerId].GetButton("GetMoreInfoPressed");
        }

        public bool IsPausePressed(int playerId = 0)
        {
            return players[playerId].GetButtonDown("Pause");
        }

        // Camera input with Y-axis inversion support
        public Vector2 GetCameraInput(int playerId = 0)
        {
            var input = new Vector2(
                players[playerId].GetAxis("AimHorizontal") * controllerSensitivity,
                players[playerId].GetAxis("AimVertical") * controllerSensitivity
            );

            // Apply inversion if enabled
            if (invertYAxis)
                input.y = -input.y;

            return input;
        }

        // Movement input
        public Vector2 GetMovementInput(int playerId = 0)
        {
            return new Vector2(
                players[playerId].GetAxis("MoveLeftRight"),
                players[playerId].GetAxis("MoveForwardBack")
            );
        }

        public bool IsPersistanceKeyPressed()
        {
            return players[0].GetButtonDown("Persistence");
        }

        public bool IsDeletionKeyPressed()
        {
            return players[0].GetButtonDown("Deletion");
        }

        public bool IsResetHeld()
        {
            return players[0].GetButton("Interact");
        }

        public bool IsSaveDebugKeyPressed()
        {
            return players[0].GetButtonDown("SaveDebug");
        }

        public bool IsMineMouseButtonPressed()
        {
            return players[0].GetButton("MinePressed");
        }

        // Weapon wheel scroll - needs special handling
        public bool IsChangingWeapons(int playerId = 0)
        {
            // Check both direct scroll input and mapped buttons
            var scrollValue = ReInput.players.GetPlayer(playerId).GetAxis("ChangeTool");
            var nextTool = ReInput.players.GetPlayer(playerId).GetButtonDown("NextTool");
            var prevTool = ReInput.players.GetPlayer(playerId).GetButtonDown("PrevTool");

            return scrollValue != 0 || nextTool || prevTool;
        }

        public int GetWeaponChangeDirection(int playerId = 0)
        {
            var scrollValue = ReInput.players.GetPlayer(playerId).GetAxis("ChangeTool");
            var nextTool = ReInput.players.GetPlayer(playerId).GetButtonDown("NextTool");
            var prevTool = ReInput.players.GetPlayer(playerId).GetButtonDown("PrevTool");

            // Check direct scroll input first
            if (scrollValue != 0)
                return scrollValue > 0 ? 1 : -1;

            // Then check dedicated buttons
            if (nextTool)
                return 1;
            if (prevTool)
                return -1;

            return 0;
        }
    }
}