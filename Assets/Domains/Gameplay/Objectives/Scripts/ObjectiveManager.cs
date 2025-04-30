using UnityEngine;

namespace Domains.Gameplay.Objectives.Scripts
{
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] public Sprite nullObjectiveImage;
        public static ObjectiveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            else if (Instance != this) Destroy(gameObject);
        }
    }
}