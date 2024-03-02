using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public delegate void SceneLoad(int scene);
    public static SceneLoad loadScene;

    void LoadScene(int scene) => SceneManager.LoadScene(scene);

    private void OnEnable() => loadScene += LoadScene;
    private void OnDisable() => loadScene -= LoadScene;
}
