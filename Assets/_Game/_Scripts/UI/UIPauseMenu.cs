using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    private PlayerInputActions _playerInputActions;
    public static bool GameIsPaused = false;

    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.MainShip.Enable();
        _playerInputActions.MainShip.Pause.performed += Pause;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (GameIsPaused)
        {
            Resume();
            return;
        }

        PlayerMainShipController.Instance.PlayerInputActions.Disable();
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {

        PlayerMainShipController.Instance.PlayerInputActions.Enable();
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void ExitToStartMenu()
    {
        PlayerMainShipController.Instance.PlayerInputActions.Disable();
        _playerInputActions.Disable();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}