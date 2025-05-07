using Digger.Modules.Runtime.Sources;
using Domains.Scene.StaticScripts;
using Domains.UI_Global.Events;
using UnityEngine;

public class PersistentDiggerMasterHelper : MonoBehaviour
{
    private DiggerMasterRuntime diggerMasterRuntime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        diggerMasterRuntime = GetComponent<DiggerMasterRuntime>();
        if (diggerMasterRuntime == null)
        {
            Debug.LogError("DiggerMasterRuntime component not found on this GameObject.");
            return;
        }

        if (GameLoadFlags.IsNewGame)
        {
            diggerMasterRuntime.DeleteAllPersistedData();
            // diggerMasterRuntime.ClearScene();
            GameLoadFlags.IsNewGame = false; // Clear the flag to avoid repeating
        }
    }

    private void OnApplicationQuit()
    {
        // Save the digger master data when the application quits
        if (diggerMasterRuntime != null) diggerMasterRuntime.PersistAll();
    }

    public void PersistDiggerMaster()
    {
        // Save the digger master data
        if (diggerMasterRuntime != null)
        {
            diggerMasterRuntime.PersistAll();
            AlertEvent.Trigger(AlertReason.SavingGame, "Digger Data Persisted", "Persisted Digger Data");
        }
    }
}