using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using NPlayer;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    private Player _player;
    private PlayerInputActions _playerInputActions;
    public static bool GameIsPaused = false;

    private void Start()
    {
        _player = FindObjectOfType<Player>();

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Pause.performed += Pause;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (GameIsPaused)
        {
            Resume();
            return;
        }

        _player.PlayerInputActions.Disable();
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        _player.PlayerInputActions.Enable();
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void ExitToStartMenu()
    {
        _player.PlayerInputActions.Disable();
        _playerInputActions.Disable();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}