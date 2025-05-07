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


        public GameObject GetTerrainPrefab(int textureIndex)
        {
            if (terrainBehavior == null)
            {
                UnityEngine.Debug.LogError("TerrainBehavior is not assigned.");
                return null;
            }

            foreach (var terrain in terrainBehavior.terrainDigParticlePrefabs)
                if (terrain.terrainLayerIndex == textureIndex)
                    return terrain.primaryPrefab;


            return terrainBehavior.defaultDigParticlePrefab;
        }
    }
}