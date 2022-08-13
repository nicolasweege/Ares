using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private float _leanTweenDuration;
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private float _defaultSoundsVolume;

    private void Awake()
    {
        LeanTween.reset();
        Utils.DisableMouse();
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
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(420, -1060, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenMainMenu);
    }

    public void _SelectButtonAnim(GameObject button) {
        LeanTween.moveLocalX(button, 50, 0.15f).setEaseOutExpo();
    }

    public void _DeselectButtonAnim(GameObject button) {
        LeanTween.moveLocalX(button, 0, 0.15f).setEaseOutExpo();
    }

#region Sound Functions
    public void _PlayUIClickSound() {
        SoundManager.PlaySound(SoundManager.Sound.UIButtonClick, _defaultSoundsVolume);
    }

    public void _PlayUIClickSound(float volume) {
        SoundManager.PlaySound(SoundManager.Sound.UIButtonClick, volume);
    }

    public void _PlayUISelectionSound() {
        SoundManager.PlaySound(SoundManager.Sound.UIButtonSelection, _defaultSoundsVolume);
    }

    public void _PlayUISelectionSound(float volume) {
        SoundManager.PlaySound(SoundManager.Sound.UIButtonSelection, volume);
    }
#endregion

    public void PlayGame() {
        Utils.EnableMouse();
        SceneManager.LoadScene("Afrodite Fight");
    }

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