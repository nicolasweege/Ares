using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Afrodite : Singleton<Afrodite> {
    #region Variables
    [Header("General")]
    public int Health;
    public GameObject DeathAnim;
    [Range(0f, 100f)] public float TurnSpeed;
    public GameObject LaserBeam;
    [SerializeField] private FlashHitEffect _flashHitEffect;
    public Transform MovePointCenter;
    public List<Transform> MovePoints = new List<Transform>();
    [NonSerialized] public Vector2 Velocity = Vector2.zero;
    [NonSerialized] public bool IsFlashing = false;

    #region Timers
    [Header("Timers")]
    [SerializeField] private float _timeToSecondStage;
    private float _secondStageTimer;
    [SerializeField] private float _timeToThirdStage;
    private float _thirdStageTimer;
    [SerializeField] private float _timeToFourthStage;
    private float _fourthStageTimer;
    #endregion

    #region State 2
    [Header("First Stage")]
    [NonSerialized] public Vector3 CurrentFirstStageProjectileDir;
    public GameObject FirstStageProjectile;
    public Transform FirstStageProjectileStartingPoint1;
    public Transform FirstStageProjectileStartingPoint2;
    public Transform FirstStageProjectileDir1;
    public Transform FirstStageProjectileDir2;
    [Range(0f, 100f)] public float FirstStageProjectileTurnSpeed;
    public Transform TurretTransform1;
    public Transform TurretTransform2;
    #endregion

    #region State 3
    [Header("Third Stage")]
    public GameObject ThirdStageProjectile;
    public GameObject ThirdStageShootAnim;
    public List<Transform> ThirdStageFirstWaveShootDirections = new List<Transform>();
    public List<Transform> ThirdStageSecondWaveShootDirections = new List<Transform>();
    #endregion

    #region State 4
    [Header("Fourth Stage")]
    public Transform FourthStageMovePointLeft;
    public Transform FourthStageMovePointRight;
    public Transform FourthStageAimPointUp;
    public Transform FourthStageAimPointDown;
    public GameObject FourthStageSatellite;
    public Transform FourthStageSatelliteUpPoint;
    public Transform FourthStageSatelliteDownPoint;
    #endregion

    #region States
    [NonSerialized] public Afrodite_State CurrentState;
    [NonSerialized] public Afrodite_State_Idle IdleState = new Afrodite_State_Idle();
    [NonSerialized] public Afrodite_State_Death DeathState = new Afrodite_State_Death();
    [NonSerialized] public Afrodite_State_1 FirstState = new Afrodite_State_1();
    [NonSerialized] public Afrodite_State_2 SecondState = new Afrodite_State_2();
    [NonSerialized] public Afrodite_State_3 ThirdState = new Afrodite_State_3();
    [NonSerialized] public Afrodite_State_4 FourthState = new Afrodite_State_4();
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
            renderer.gameObject.AddComponent<Afrodite_Reset_Color>();
    }

    private void Update() {
        Debug.Log($"Health: {Health} / Current State: {CurrentState}");
        
        IsFlashing = _flashHitEffect.IsFlashing;
        HandleAI();
        CurrentState.UpdateState(this);

        if (Health <= 0 && CurrentState != DeathState)
            SwitchState(DeathState);
    }

    private void HandleAI() {
        if (Health <= 50 && Health > 0) {
            if (CurrentState != SecondState && CurrentState != ThirdState && CurrentState != FourthState) {
                _secondStageTimer -= Time.deltaTime;
                if (_secondStageTimer <= 0f) {
                    SwitchState(SecondState);
                    _secondStageTimer = _timeToSecondStage;
                }

                _thirdStageTimer -= Time.deltaTime;
                if (_thirdStageTimer <= 0f) {
                    SwitchState(ThirdState);
                    _thirdStageTimer = _timeToThirdStage;
                }

                _fourthStageTimer -= Time.deltaTime;
                if (_fourthStageTimer <= 0f) {
                    SwitchState(FourthState);
                    _fourthStageTimer = _timeToFourthStage;
                }
            }
        }

        if (Health <= 70 && Health > 50) {
            if (CurrentState != SecondState && CurrentState != FourthState) {
                _secondStageTimer -= Time.deltaTime;
                if (_secondStageTimer <= 0f) {
                    SwitchState(SecondState);
                    _secondStageTimer = _timeToSecondStage;
                }

                _fourthStageTimer -= Time.deltaTime;
                if (_fourthStageTimer <= 0f) {
                    SwitchState(FourthState);
                    _fourthStageTimer = _timeToFourthStage;
                }
            }
        }

        if (Health <= 100 && Health > 70) {
            if (CurrentState != SecondState) {
                _secondStageTimer -= Time.deltaTime;
                if (_secondStageTimer <= 0f) {
                    SwitchState(SecondState);
                    _secondStageTimer = _timeToSecondStage;
                }
            }
        }
    }

    public void SwitchState(Afrodite_State state) {
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

    public void TakeDamage(int damage) {
        Health -= damage;
        _flashHitEffect.Flash();
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }
    
    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}