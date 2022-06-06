using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState State { get; private set; }
    public static event Action<GameState> OnBeforeGameStateChanged;
    public static event Action<GameState> OnAfterGameStateChanged;

    public void UpdateGameState(GameState newState)
    {
        OnBeforeGameStateChanged?.Invoke(newState);

        State = newState;

        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
        }

        OnAfterGameStateChanged?.Invoke(newState);
    }

    private void HandlePaused()
    {
        
    }

    private void HandleStarting()
    {
        
    }
}

[Serializable] public enum GameState
{
    Starting = 0,
    Paused = 1
}