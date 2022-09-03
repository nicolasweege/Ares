using System;
using UnityEngine;
using UnityEngine.Events;

public class ErosController : Singleton<ErosController> {
    #region Variables
    [Header("General")]
    public int Health;
    public GameObject DeathAnim;
    [SerializeField] private FlashHitEffect _flashHitEffect;
    [NonSerialized] public bool IsFlashing = false;

    #region Stage States Variables
    [NonSerialized] public ErosBaseState CurrentState;
    [NonSerialized] public ErosIdleState IdleState = new ErosIdleState();
    [NonSerialized] public ErosDeathState DeathState = new ErosDeathState();
    #endregion

    [Header("Events")]
    public UnityEvent ScreenShakeEvent;
    #endregion

    protected override void Awake() {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        CurrentState = IdleState;
        CurrentState.EnterState(this);

        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
            renderer.gameObject.AddComponent<ErosResetColor>();
    }

    private void Update() {
        Debug.Log($"Health: {Health} CurrentState: {CurrentState}");

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