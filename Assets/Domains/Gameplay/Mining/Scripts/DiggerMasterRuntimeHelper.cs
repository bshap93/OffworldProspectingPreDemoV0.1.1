using Digger.Modules.Runtime.Sources;
using UnityEngine;

namespace Domains.Gameplay.Mining.Scripts
{
    public class DiggerMasterRuntimeHelper : MonoBehaviour

    {
        [SerializeField] private DiggerMasterRuntime diggerMasterRuntime;
        [SerializeField] private Terrain terrain;

        private void Start()
        {
            if (!Application.isEditor) diggerMasterRuntime.SetupRuntimeTerrain(terrain);
        }


        public void ForceDeleteAllPersistedData()
        {
            if (diggerMasterRuntime == null)
            {
                UnityEngine.Debug.LogError("DiggerMasterRuntime is not assigned.");
                return;
            }


            diggerMasterRuntime.DeleteAllPersistedData();

            diggerMasterRuntime.SetupRuntimeTerrain(terrain);
        }
    }
}