using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Eros : Singleton<Eros> {
    #region Variables
    [Header("General")]
    public int Health;
    public GameObject MainAnimation;
    public Rotate RotateComponent;
    public GameObject SpritesGameObject;
    [SerializeField] private FlashHitEffect _flashHitEffect;
    [NonSerialized] public bool IsFlashing = false;
    [NonSerialized] public Vector2 Velocity = Vector2.zero;

    #region Timers
    [Header("Timers")]
    public float TimeToSecondState;
    private float SecondStateTimer;
    public float TimeToThirdState;
    private float ThirdStateTimer;
    #endregion

    #region Move Points
    [Header("Move Points")]
    public Transform CenterMovePoint;
    public Transform MovePointUp;
    public Transform MovePointDown;
    public Transform MovePointRight;
    public Transform MovePointLeft;
    #endregion

    #region State 1
    [Header("State 1")]
    public GameObject State_1_Bullet;
    public List<Transform> State_1_BulletDirs_1 = new List<Transform>();
    public List<Transform> State_1_BulletDirs_2 = new List<Transform>();
    #endregion

    #region States
    [NonSerialized] public Eros_State CurrentState;
    [NonSerialized] public Eros_State_Idle IdleState = new Eros_State_Idle();
    [NonSerialized] public Eros_State_Death DeathState = new Eros_State_Death();
    [NonSerialized] public Eros_State_1 FirstState = new Eros_State_1();
    [NonSerialized] public Eros_State_2 SecondState = new Eros_State_2();
    [NonSerialized] public Eros_State_3 ThirdState = new Eros_State_3();
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
            renderer.gameObject.AddComponent<Eros_Reset_Color>();
    }

    private void Update() {
        // Debug.Log($"Health: {Health} / Current State: {CurrentState}");

        IsFlashing = _flashHitEffect.IsFlashing;
        HandleAI();
        CurrentState.UpdateState(this);

        if (Health <= 0 && CurrentState != DeathState) SwitchState(DeathState);
    }

    private void HandleAI()
    {
        if (Health <= 50 && Health > 0)
        {
            if (CurrentState != SecondState && CurrentState != ThirdState)
            {
                SecondStateTimer -= Time.deltaTime;
                if (SecondStateTimer <= 0f)
                {
                    SwitchState(SecondState);
                    SecondStateTimer = TimeToSecondState;
                }

                ThirdStateTimer -= Time.deltaTime;
                if (ThirdStateTimer <= 0f)
                {
                    SwitchState(ThirdState);
                    ThirdStateTimer = TimeToThirdState;
                }
            }
        }

        if (Health <= 70 && Health > 50)
        {
            if (CurrentState != ThirdState)
            {
                ThirdStateTimer -= Time.deltaTime;
                if (ThirdStateTimer <= 0f)
                {
                    SwitchState(ThirdState);
                    ThirdStateTimer = TimeToThirdState;
                }
            }
        }
    }

    public void TakeDamage(int damage) {
        Health -= damage;
        _flashHitEffect.Flash();
    }

    public void SwitchState(Eros_State state) {
        if (enabled) {
            CurrentState = state;
            state.EnterState(this);
        }
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