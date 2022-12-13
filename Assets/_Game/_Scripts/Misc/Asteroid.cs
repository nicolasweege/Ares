using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float Speed;
    public Vector3 Direction;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * Speed; // move asteroid
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}