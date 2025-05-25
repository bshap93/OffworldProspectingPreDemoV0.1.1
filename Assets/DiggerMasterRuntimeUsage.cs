using Digger.Modules.Runtime.Sources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiggerMasterRuntimeUsage : MonoBehaviour
{
    private DiggerMasterRuntime _diggerMasterRuntime;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _diggerMasterRuntime = FindFirstObjectByType<DiggerMasterRuntime>();

        var isNewGame = PlayerPrefs.GetInt("IsNewGame", 1) == 1;

        if (isNewGame)
        {
            _diggerMasterRuntime.DeleteAllPersistedData();

            PlayerPrefs.SetInt("IsNewGame", 0);

            _diggerMasterRuntime.PersistAll();
            // Reload the scene to apply changes
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnApplicationQuit()
    {
        _diggerMasterRuntime.PersistAll();
    }
}