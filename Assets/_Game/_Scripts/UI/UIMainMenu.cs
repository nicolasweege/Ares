using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void OpenMenu()
    {
        // transform.LeanMove(new Vector2())
    }

    public void CloseMenu()
    {

    }

    public void PlayGame() => SceneManager.LoadScene("Afrodite Fight");

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}