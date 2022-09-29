using System.Linq;
using Mayotech.Navigation;
using UnityEngine.SceneManagement;

public static class SceneExtensions
{
    public static SceneController GetSceneController(this Scene scene)
    {
        var root = scene.GetRootGameObjects();
        return root.FirstOrDefault(item => item.GetComponent<SceneController>() != null)
            ?.GetComponent<SceneController>() ?? null;
    }
}