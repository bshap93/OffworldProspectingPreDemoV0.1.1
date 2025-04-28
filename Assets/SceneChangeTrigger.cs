using MoreMountains.Tools;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        MMSceneLoadingManager.LoadScene(sceneName);
    }
}