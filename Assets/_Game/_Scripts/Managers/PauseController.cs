using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour {
    private PlayerInputActions _inputActions;

    private void Awake() {
        _inputActions = new PlayerInputActions();
        _inputActions.UI.Enable();
        _inputActions.UI.Pause.performed += Pause_Performed;
    }

    private void Pause_Performed(InputAction.CallbackContext context) {
        switch (GameManager.Instance.CurrentState) {
            case GameState.DeathMenu:
            case GameState.WinState:
                return;
        }

        GameState currentState = GameManager.Instance.CurrentState;
        GameState newState = currentState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay;
        GameManager.Instance.SetGameState(newState);
    }

    private void OnDestroy() {
        _inputActions.UI.Pause.performed -= Pause_Performed;
    }
}