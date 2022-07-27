using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageSatelliteResetColor : MonoBehaviour
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
        if (!AfroditeFourthStageSatelliteController.Instance.IsFlashing)
        {
            _spriteRenderer.color = _originalColor;
        }
    }
}