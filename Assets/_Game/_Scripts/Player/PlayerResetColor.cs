using UnityEngine;

public class PlayerResetColor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void Update()
    {
        if (PlayerMainShipController.Instance.CanResetColors)
        {
            _spriteRenderer.color = _originalColor;
        }
    }
}