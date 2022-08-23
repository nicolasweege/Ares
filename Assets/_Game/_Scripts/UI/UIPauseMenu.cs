using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private float _leanTweenDuration;
    [SerializeField] private GameObject _mainMenuComponents, _optionsMenuComponents;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private float _defaultSoundsVolume;
    [SerializeField] private float _buttonSelectedScaleBuffer;
    [SerializeField] private Renderer2DData _renderer2DData;
    [SerializeField] private Animator _sceneTransition;
    [SerializeField] private int _sceneTransitionTime;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    #region Menu Functions
    public void HandlePause()
    {
        DisablePlayerInput();
        EnableUIInput();
        _mainMenuComponents.SetActive(true);
        // AudioListener.pause = true;
    }

    public void HandleResume()
    {
        EnablePlayerInput();
        DisableUIInput();
        _mainMenuComponents.SetActive(false);
        // AudioListener.pause = false;
    }

    public void ExitToMainMenu()
    {
        // AudioListener.pause = false;
        _renderer2DData.rendererFeatures[0].SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }

    public async void HandleExit() {
        _sceneTransition.SetTrigger("Start");
        await Task.Delay(_sceneTransitionTime);
        // AudioListener.pause = false;
        _renderer2DData.rendererFeatures[0].SetActive(false);
        SceneManager.LoadScene("Main Menu");
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
    #endregion

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        if (newState == GameState.Paused) {
            HandlePause();
        } else {
            HandleResume();
        }
    }
}