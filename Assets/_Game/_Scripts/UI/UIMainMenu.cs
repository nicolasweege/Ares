using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private float _leanTweenDuration;
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EventSystem _eventSystem;

    private void Awake()
    {
        LeanTween.reset();
    }

    public void OpenMainMenu()
    {
        EnableUIInput();
        LeanTween.moveLocal(_mainMenuComponents, new Vector3(0, 0, 0), _leanTweenDuration).setEaseOutExpo();
    }

    public void CloseMainMenu()
    {
        DisableUIInput();
        LeanTween.moveLocal(_mainMenuComponents, new Vector3(-1200, 0, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenOptionsMenu);
    }

    public void OpenOptionsMenu()
    {
        EnableUIInput();
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(0, 0, 0), _leanTweenDuration).setEaseOutExpo();
    }

    public void CloseOptionsMenu()
    {
        DisableUIInput();
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(1200, 0, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenMainMenu);
    }

    public void ButtonSelected(Button button)
    {
        LeanTween.moveLocal(button.gameObject, new Vector3(0, 5, 0), _leanTweenDuration).setEaseOutBounce();
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

    public void EnableUIInput()
    {
        _canvasGroup.interactable = true;
    }

    public void DisableUIInput()
    {
        _canvasGroup.interactable = false;
    }
}