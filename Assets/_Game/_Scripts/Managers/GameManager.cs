using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable] public enum GameState {
    Gameplay,
    Paused,
    DeathMenu,
    WinState
}

public class GameManager : Singleton<GameManager> {
    [SerializeField] private GameState _startingGameState;
    [SerializeField] private Renderer2DData _renderer2DData;

    public GameState CurrentState { get; private set; }
    public delegate void GameStateChangeHandler(GameState newState);
    public static event GameStateChangeHandler OnBeforeGameStateChanged;
    public static event GameStateChangeHandler OnAfterGameStateChanged;

    protected override void Awake() {
        base.Awake();
        SetGameState(_startingGameState);
    }

    public void SetGameState(GameState newState) {
        if (newState == CurrentState)
            return;

        OnBeforeGameStateChanged?.Invoke(newState);

        CurrentState = newState;

        switch (newState) {
            case GameState.Gameplay:
                _renderer2DData.rendererFeatures[0].SetActive(true);
                break;

            case GameState.DeathMenu:
                _renderer2DData.rendererFeatures[0].SetActive(false);
                AudioListener.pause = true;
                CinemachineManager.Instance.SetTargetTransform(PlayerController.Instance.transform);
                UIPauseMenuController.Instance.DisablePlayerHud();
                FunctionTimer.Create(() => CinemachineManager.Instance.ZoomIn(5f, 3f), 0.1f);
                FunctionTimer.Create(() => CinematicBars.Instance.Show(500f, 0.5f), 0.5f);
                FunctionTimer.Create(UIPauseMenuController.Instance.HandleDeathMenu, 1.4f);
                break;

            case GameState.WinState:
                _renderer2DData.rendererFeatures[0].SetActive(false);
                UIPauseMenuController.Instance.DisablePlayerHud();
                FunctionTimer.Create(() => CinematicBars.Instance.Show(1500f, 0.5f), 1f);
                FunctionTimer.Create(() => LevelManager.Instance.LoadScene("MainMenu"), 2.5f);
                break;

            case GameState.Paused:
                _renderer2DData.rendererFeatures[0].SetActive(false);
                break;
        }

        OnAfterGameStateChanged?.Invoke(newState);
    }

    private void OnDestroy() {
        FunctionTimer.ClearTimers();
    }
}