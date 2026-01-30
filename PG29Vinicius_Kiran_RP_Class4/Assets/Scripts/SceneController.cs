using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "NextLevel";
    public void LoadNextScene(string sceneToLoad)
    {
        sceneToLoad = nextSceneName;
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
