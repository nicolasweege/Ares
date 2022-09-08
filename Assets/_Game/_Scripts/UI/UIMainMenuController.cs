using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIMainMenuController : Singleton<UIMainMenuController> {
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private float _defaultSoundsVolume;
    [SerializeField] private Color _buttonSelectedColor;
    [SerializeField] private Color _buttonDeselectedColor;

    protected override void Awake() {
        LeanTween.reset();
        AudioListener.pause = false;
        AssetsManager.Instance.PlayerIsNotTakingDamageSnapshot.TransitionTo(0f);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    #region Animation Functions
    public void _SelectButtonAnim(TMP_Text buttonText) {
        buttonText.color = _buttonSelectedColor;
    }

    public void _DeselectButtonAnim(TMP_Text buttonText) {
        buttonText.color = _buttonDeselectedColor;
    }
    #endregion

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

    public void _PlayUILightClickSound() {
        SoundManager.PlaySound(SoundManager.Sound.UIButtonLightClick, _defaultSoundsVolume);
    }

    public void _PlayUILightClickSound(float volume) {
        SoundManager.PlaySound(SoundManager.Sound.UIButtonLightClick, volume);
    }
    #endregion

    public void EnableUIInput() {
        _eventSystem.sendNavigationEvents = true;
        _canvasGroup.interactable = true;
    }

    public void DisableUIInput() {
        _eventSystem.sendNavigationEvents = false;
        _canvasGroup.interactable = false;
    }

    public void LoadScene(string sceneName) {
        LevelManager.Instance.LoadScene(sceneName);
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}