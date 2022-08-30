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
        OnAfterGameStateChanged?.Invoke(newState);
    }
}

[Serializable] public enum GameState {
    Gameplay,
    Paused,
    DeathMenu
}