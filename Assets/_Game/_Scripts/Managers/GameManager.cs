using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UIPauseMenu _UIPauseMenu;
    [SerializeField] private GameState _startingGameState; 

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
            case GameState.DeathMenu:
                CinemachineManager.Instance.SetTargetTransform(PlayerController.Instance.transform);
                FunctionTimer.Create(() => CinemachineManager.Instance.ZoomIn(5f, 3f), 0.1f);
                FunctionTimer.Create(() => CinematicBars.Instance.Show(500f, 0.5f), 0.5f);
                FunctionTimer.Create(_UIPauseMenu.HandleDeathMenu, 1.4f);
                break;
        }

        OnAfterGameStateChanged?.Invoke(newState);
    }

    public void HandleWonLevel() {
        FunctionTimer.Create(_UIPauseMenu.SetWonTextActive, 0.5f);
        FunctionTimer.Create(() => CinematicBars.Instance.Show(500f, 0.8f), 0.5f);
    }
}

[Serializable] public enum GameState {
    Gameplay,
    Paused,
    DeathMenu
}