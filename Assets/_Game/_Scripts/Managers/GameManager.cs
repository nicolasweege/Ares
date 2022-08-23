using System;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; }
    public delegate void GameStateChangeHandler(GameState newState);
    public static event GameStateChangeHandler OnBeforeGameStateChanged;
    public static event GameStateChangeHandler OnAfterGameStateChanged;

    public void SetGameState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        OnBeforeGameStateChanged?.Invoke(newState);

        CurrentState = newState;

        switch (newState)
        {
            case GameState.Gameplay:
                break;
            case GameState.Paused:
                break;
        }

        OnAfterGameStateChanged?.Invoke(newState);
    }
}

[Serializable] public enum GameState
{
    Gameplay,
    Paused
}