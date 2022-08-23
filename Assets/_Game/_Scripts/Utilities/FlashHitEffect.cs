using System;
using System.Collections;
using UnityEngine;

public class FlashHitEffect : MonoBehaviour
{
    [SerializeField] private Color _flashColor;
    [SerializeField] private float _duration;
    private Coroutine _flashRoutine;
    [NonSerialized] public bool IsFlashing = false;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
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
            float a = spr.color.a;
            spr.color = new Color(_flashColor.r, _flashColor.g, _flashColor.b, a);
        }

        IsFlashing = true;

        yield return new WaitForSeconds(_duration);

        IsFlashing = false;
        _flashRoutine = null;
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}