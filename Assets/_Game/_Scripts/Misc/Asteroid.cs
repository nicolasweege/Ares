using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float Speed;
    public Vector3 Direction;

    private void Awake()
    {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        transform.position += Direction * Time.deltaTime * Speed; // move asteroid

        // destroy asteroid when outside the screen
        if (transform.position.y < 7)
        {
            if (transform.position.x > 14 || transform.position.x < -14) Destroy(gameObject);
        }

        if (transform.position.y < -9) Destroy(gameObject);
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}