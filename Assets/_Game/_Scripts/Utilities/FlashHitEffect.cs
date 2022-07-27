using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashHitEffect : MonoBehaviour
{
    // [SerializeField] private Material _flashMaterial;
    [SerializeField] private Color _flashColor;
    [SerializeField] private float _duration;
    // [SerializeField] private SpriteRenderer _spriteRenderer;
    // private Material _originalMaterial;
    // private Color _originalColor;
    private Coroutine _flashRoutine;

    void Start()
    {
        // _originalMaterial = _spriteRenderer.material;
    }

    public void Flash()
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }

        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {

        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
        {
            spr.color = _flashColor;
        }
        AfroditeController.Instance.IsFlashing = true;
        // _spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(_duration);
        // _spriteRenderer.material = _originalMaterial;
        AfroditeController.Instance.IsFlashing = false;
        _flashRoutine = null;
    }
}