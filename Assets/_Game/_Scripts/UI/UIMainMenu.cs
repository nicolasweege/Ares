using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private float _leanTweenDuration;
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents, _levelsMenuComponents;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private float _defaultSoundsVolume;
    [SerializeField] private float _buttonSelectedScaleBuffer;

    private void Awake()
    {
        LeanTween.reset();
        Utils.DisableMouse();
    }

    public void OpenMainMenu()
    {
        LeanTween.moveLocal(_mainMenuComponents, new Vector3(0, 0, 0), _leanTweenDuration).setEaseOutExpo().setOnComplete(EnableUIInput);
    }

    public void CloseMainMenu()
    {
        DisableUIInput();
        LeanTween.moveLocal(_mainMenuComponents, new Vector3(-1200, 0, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenOptionsMenu);
    }

    public void OpenOptionsMenu()
    {
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(0, 0, 0), _leanTweenDuration).setEaseOutExpo().setOnComplete(EnableUIInput);
    }

    public void OpenLevelsMenu() {
        DisableUIInput();
        LeanTween.moveLocal(_mainMenuComponents, new Vector3(-1200, 0, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenLevelsMenuAnim);
    }

    public void OpenLevelsMenuAnim() {
        LeanTween.moveLocal(_levelsMenuComponents, new Vector3(0, 0, 0), _leanTweenDuration).setEaseOutExpo().setOnComplete(EnableUIInput);
    }

    public void CloseLevelsMenu() {
        DisableUIInput();
        LeanTween.moveLocal(_levelsMenuComponents, new Vector3(960, -1060, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenMainMenu);
    }

    public void CloseOptionsMenu()
    {
        DisableUIInput();
        LeanTween.moveLocal(_optionsMenuComponents, new Vector3(420, -1060, 0), _leanTweenDuration).setEaseInExpo().setOnComplete(OpenMainMenu);
    }

    public void _SelectButtonAnim(GameObject button) {
        LeanTween.scale(button, new Vector3(_buttonSelectedScaleBuffer, _buttonSelectedScaleBuffer, 0), 0.15f);
    }

    public void _DeselectButtonAnim(GameObject button) {
        LeanTween.scale(button, new Vector3(1f, 1f, 0), 0.15f);
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

    public void LoadScene(string sceneName) {
        Utils.EnableMouse();
        SceneManager.LoadScene(sceneName);
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
        _eventSystem.sendNavigationEvents = true;
        _canvasGroup.interactable = true;
    }

    public void DisableUIInput()
    {
        _eventSystem.sendNavigationEvents = false;
        _canvasGroup.interactable = false;
    }
}