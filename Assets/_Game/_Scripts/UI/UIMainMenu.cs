using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private float _leanTweenDuration;
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents;

    private void Awake()
    {
        LeanTween.reset();
    }

    public void OpenMainMenu()
    {
        LeanTween.moveLocal(_mainMenuComponents, new Vector3(0, 0, 0), _leanTweenDuration).setEaseOutExpo();
    }

    public void CloseMainMenu()
    {
        LeanTween.moveLocal(_mainMenuComponents, new Vector3(-1200, 0, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenOptionsMenu);
    }

    public void OpenOptionsMenu()
    {
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(0, 0, 0), _leanTweenDuration).setEaseOutExpo();
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(0, 10f, 0), _leanTweenDuration).setDelay(0.25f);
    }

    public void CloseOptionsMenu()
    {
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(1200, 0, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenMainMenu);
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