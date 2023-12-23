using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName; // The name of the scene to load

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
