using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public static void NextScene() {
        int index = SceneManager.GetActiveScene().buildIndex;
        index++;

        if (index < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(index);
        }
    }

    public static void PrevScene() {
        int index = SceneManager.GetActiveScene().buildIndex;
        index--;

        if (index >= 0) {
            SceneManager.LoadScene(index);
        }
    }

    public static void ResetGame() {
        SceneManager.LoadScene(0);
    }

    public static void ExitGame() {
        // Application.Quit() doesn't work in the editor, so stop play mode instead
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
