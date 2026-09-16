using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvas : MonoBehaviour
{
    public void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
