using UnityEngine;

public class PauseController : MonoBehaviour
{
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameState currentState = GameManager.Instance.CurrentState;
            GameState newState = currentState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay;

            GameManager.Instance.SetGameState(newState);
        }
    }
}