using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ErosController : Singleton<ErosController> {
    #region Variables
    [Header("General")]
    public int Health;
    public GameObject DeathAnim;
    public Rotate RotateComponent;
    [SerializeField] private FlashHitEffect _flashHitEffect;
    [NonSerialized] public bool IsFlashing = false;
    [NonSerialized] public Vector2 Velocity = Vector2.zero;

    #region First Stage Variables
    [Header("First Stage")]
    public GameObject FirstStageBullet;
    public List<Transform> FirstStageBulletDirs = new List<Transform>();
    #endregion

    #region State Variables
    [NonSerialized] public ErosBaseState CurrentState;
    [NonSerialized] public ErosIdleState IdleState = new ErosIdleState();
    [NonSerialized] public ErosDeathState DeathState = new ErosDeathState();
    [NonSerialized] public ErosFirstStageState FirstStageState = new ErosFirstStageState();
    #endregion

    [Header("Events")]
    public UnityEvent ScreenShakeEvent;
    #endregion

    protected override void Awake() {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        CurrentState = IdleState;
        CurrentState.EnterState(this);

        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()) {
            renderer.gameObject.AddComponent<ErosResetColor>();
        }
    }

    private void Update() {
        Debug.Log($"Health: {Health} / Current State: {CurrentState}");

        IsFlashing = _flashHitEffect.IsFlashing;
        CurrentState.UpdateState(this);

        if (Health <= 0 && CurrentState != DeathState) {
            SwitchState(DeathState);
        }
    }

    public void TakeDamage(int damage) {
        Health -= damage;
        _flashHitEffect.Flash();
    }

    public void SwitchState(ErosBaseState state) {
        CurrentState = state;
        state.EnterState(this);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        CurrentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        CurrentState.OnTriggerExit(this, other);
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }
    
    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}