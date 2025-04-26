using Domains.Scene.Terrain.Scripts;
using UnityEngine;

namespace Domains.Scripts_that_Need_Sorting
{
    public class TerrainController : MonoBehaviour
    {
        [SerializeField] public TerrainBehavior terrainBehavior;

        public static TerrainController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
    }
}