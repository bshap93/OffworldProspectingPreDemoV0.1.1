using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using Domains.Player.Events;
using Domains.Scene.Scripts;
using Domains.UI_Global.Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

namespace Domains.Gameplay.Managers.Scripts
{
    public class DiggerDataManager : Manager, MMEventListener<DiggerEvent>
    {
        private const string AutoSaveKey = "AutoSave";
        private const string ForceDeleteOnStartKey = "ForceDeleteOnStart";

        public static bool AutoSave = true;
        public static bool ForceDeleteOnStart;
        public DiggerMasterRuntime diggerMasterRuntime;

        public MMFeedbacks deleteAllDataFeedbacks;
        public MMFeedbacks saveDataFeedbacks;


        public DiggerSystem[] diggerSystems;
        private string _savePath;
        public static DiggerDataManager Instance { get; private set; }

        private void Awake()
        {
            LoadBooleanFlags();
            if (diggerMasterRuntime == null)
                UnityEngine.Debug.LogError("DiggerDataManager: No DiggerMasterRuntime found in scene!");

            if (ForceDeleteOnStart) DeleteAllDiggerData();


            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            DiggerSaveUtility.Save(true, false);
        }


        private void OnEnable()
        {
            this.MMEventStartListening();
        }

        private void OnDisable()
        {
            this.MMEventStopListening();
        }

        private void OnApplicationQuit()
        {
            // if (forceDeleteOnQuit) DeleteAllDiggerData();

            if (AutoSave) SaveDiggerData();
        }

        public void OnMMEvent(DiggerEvent eventType)
        {
            switch (eventType.EventType)
            {
                case DiggerEventType.Persist:
                    SaveDiggerData();
                    break;
                case DiggerEventType.Delete:
                    DeleteAllDiggerData();
                    break;
            }
        }


        public void SaveDiggerData()
        {
            saveDataFeedbacks?.PlayFeedbacks();
            diggerMasterRuntime.PersistAll();

            AlertEvent.Trigger(AlertReason.SavingGame,
                "Persisting digger data...", "Saving digger data...");
            UnityEngine.Debug.Log("Digger data saved.");

            ForceDeleteOnStart = false; // Reset the flag after saving
            DiggerSaveUtility.Save(AutoSave, ForceDeleteOnStart);
        }

        public void DeleteAllDiggerData()
        {
            deleteAllDataFeedbacks?.PlayFeedbacks();
            if (diggerMasterRuntime == null)
            {
                UnityEngine.Debug.Log("DiggerDataManager: No DiggerMasterRuntime found in scene!");
                return;
            }

            diggerMasterRuntime.DeleteAllPersistedData();
            AlertEvent.Trigger(AlertReason.DeletingDiggerData, "Digger data deleted.");

            UnityEngine.Debug.Log("Digger data deleted.");
        }

        protected override void LoadBooleanFlags()
        {
            if (_savePath == null) _savePath = GetSaveFilePath();

            if (ES3.KeyExists(AutoSaveKey, _savePath))
            {
                AutoSave = ES3.Load<bool>(AutoSaveKey, _savePath);
                UnityEngine.Debug.Log($"Loaded AutoSave state: {AutoSave}");
            }
            else
            {
                AutoSave = false;
                UnityEngine.Debug.Log($"No saved AutoSave state found. Defaulting to: {AutoSave}");
            }

            if (ES3.KeyExists(ForceDeleteOnStartKey, _savePath))
            {
                ForceDeleteOnStart = ES3.Load<bool>(ForceDeleteOnStartKey, _savePath);
                UnityEngine.Debug.Log($"Loaded ForceDeleteOnStart state: {ForceDeleteOnStart}");
            }
            else
            {
                // Set to true as we do want to delete on start
                ForceDeleteOnStart = true;
            }
        }
    }
}