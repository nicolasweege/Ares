using UnityEngine;

public class PlayerResetColor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void Update()
    {
        if (PlayerController.Instance.CanResetColors)
        {
            _spriteRenderer.color = _originalColor;
        }
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}