using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private float _defaultSoundsVolume;
    [SerializeField] private Renderer2DData _renderer2DData;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Color _buttonSelectedColor;
    [SerializeField] private Color _buttonDeselectedColor;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    #region Menu Functions
    public void HandlePause()
    {
        EnableUIInput();
        DisablePlayerInput();
        _mainMenuComponents.SetActive(true);
        AudioListener.pause = true;
    }

    public void HandleResume()
    {
        DisableUIInput();
        EnablePlayerInput();
        _mainMenuComponents.SetActive(false);
        AudioListener.pause = false;
    }

    public void ExitToMainMenu()
    {
        _renderer2DData.rendererFeatures[0].SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
    #endregion

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

    public void EnablePlayerInput() {
        PlayerController.Instance.PlayerInputActions.Enable();
    }

    public void DisablePlayerInput() {
        PlayerController.Instance.PlayerInputActions.Disable();
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

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void SetPuasedGameState() {
        GameManager.Instance.SetGameState(GameState.Paused);
    }

    public void SetGameplayGameState() {
        GameManager.Instance.SetGameState(GameState.Gameplay);
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        if (newState == GameState.Paused) {
            HandlePause();
            _resumeButton.Select();
            _renderer2DData.rendererFeatures[0].SetActive(false);
        } else {
            HandleResume();
            _renderer2DData.rendererFeatures[0].SetActive(true);
        }
    }
}