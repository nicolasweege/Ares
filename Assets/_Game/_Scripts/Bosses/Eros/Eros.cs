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
    [SerializeField] private float _timeToSecondStage;
    private float _secondStageTimer;
    [SerializeField] private float _timeToThirdStage;
    private float _thirdStageTimer;
    #endregion

    #region Move Points
    [Header("Move Points")]
    public Transform NullMovePoint;
    public Transform MovePointUp;
    public Transform MovePointDown;
    public Transform MovePointRight;
    public Transform MovePointLeft;
    #endregion

    #region State 1
    [Header("First Stage")]
    public GameObject FirstStageBullet;
    public List<Transform> FirstStageBulletDirs_1 = new List<Transform>();
    public List<Transform> FirstStageBulletDirs_2 = new List<Transform>();
    #endregion

    #region State 2
    [Header("Second Stage")]
    public GameObject SecondStageBullet;
    #endregion

    #region State 3
    [Header("Third Stage")]
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

        if (Health <= 0 && CurrentState != DeathState) 
            SwitchState(DeathState);
    }

    private void HandleAI()
    {
        if (Health <= 50 && Health > 0)
        {
            if (CurrentState != SecondState && CurrentState != ThirdState)
            {
                _secondStageTimer -= Time.deltaTime;
                if (_secondStageTimer <= 0f)
                {
                    SwitchState(SecondState);
                    _secondStageTimer = _timeToSecondStage;
                }

                _thirdStageTimer -= Time.deltaTime;
                if (_thirdStageTimer <= 0f)
                {
                    SwitchState(ThirdState);
                    _thirdStageTimer = _timeToThirdStage;
                }
            }
        }

        if (Health <= 70 && Health > 50)
        {
            if (CurrentState != ThirdState)
            {
                _thirdStageTimer -= Time.deltaTime;
                if (_thirdStageTimer <= 0f)
                {
                    SwitchState(ThirdState);
                    _thirdStageTimer = _timeToThirdStage;
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