using System;

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
                break;
            case GameState.Paused:
                break;
        }

        OnAfterGameStateChanged?.Invoke(newState);
    }
}

[Serializable] public enum GameState
{
    Starting = 0,
    Paused = 1
}