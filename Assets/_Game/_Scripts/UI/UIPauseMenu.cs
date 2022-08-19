using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private float _leanTweenDuration;
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private float _defaultSoundsVolume;
    [SerializeField] private float _buttonSelectedScaleBuffer;
    [SerializeField] private Renderer2DData _renderer2DData;
    private PlayerInputActions _playerInputActions;
    public static bool GameIsPaused = false;

    private void Awake() {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.MainShip.Enable();
        _playerInputActions.MainShip.Pause.performed += HandlePause;
    }

    #region Menu Functions
    public void HandlePause(InputAction.CallbackContext context)
    {
        if (GameIsPaused) {
            HandleResume();
            return;
        }

        EnableUIInput();
        DisablePlayerInput();
        _mainMenuComponents.SetActive(true);
        _optionsMenuComponents.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void HandleResume()
    {
        DisableUIInput();
        EnablePlayerInput();
        _mainMenuComponents.SetActive(false);
        _optionsMenuComponents.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void ExitToMainMenu()
    {
        _playerInputActions.Disable();
        Time.timeScale = 1f;
        _renderer2DData.rendererFeatures[0].SetActive(false);
        LoadScene("Main Menu");
    }
    #endregion

    #region Animation Functions
    public void _SelectButtonAnim(GameObject button) {
        LeanTween.scale(button, new Vector3(_buttonSelectedScaleBuffer, _buttonSelectedScaleBuffer, 0), 0.15f);
    }

    public void _DeselectButtonAnim(GameObject button) {
        LeanTween.scale(button, new Vector3(1f, 1f, 0), 0.15f);
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

    #region Input Functions
    public void EnablePlayerInput() {
        PlayerMainShipController.Instance.PlayerInputActions.Enable();
    }

    public void DisablePlayerInput() {
        PlayerMainShipController.Instance.PlayerInputActions.Disable();
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
    #endregion

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}