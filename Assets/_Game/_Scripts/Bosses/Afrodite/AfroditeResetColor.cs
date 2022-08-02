using UnityEngine;

public class AfroditeResetColor : MonoBehaviour
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
        if (!AfroditeController.Instance.IsFlashing)
        {
            _spriteRenderer.color = _originalColor;
        }
    }
}