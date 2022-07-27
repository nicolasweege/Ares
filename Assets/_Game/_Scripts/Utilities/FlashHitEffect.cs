using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashHitEffect : MonoBehaviour
{
    [SerializeField] private Color _flashColor;
    [SerializeField] private float _duration;
    private Coroutine _flashRoutine;
    [NonSerialized] public bool IsFlashing = false;

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
            float a = spr.color.a;
            spr.color = new Color(_flashColor.r, _flashColor.g, _flashColor.b, a);
        }

        IsFlashing = true;

        yield return new WaitForSeconds(_duration);

        IsFlashing = false;
        _flashRoutine = null;
    }
}