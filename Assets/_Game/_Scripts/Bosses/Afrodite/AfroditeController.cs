using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AfroditeController : Singleton<AfroditeController>
{
    public int Health;
    public float Speed;
    public int CollisionDamage;
    public GameObject DeathAnim;
    [Range(0f, 100f)] public float TurnSpeed;
    public GameObject LaserBeam;
    [SerializeField] private FlashHitEffect _flashHitEffect;
    public Transform MovePointCenter;
    public List<Transform> MovePoints = new List<Transform>();
    [NonSerialized] public Vector2 Velocity = Vector2.zero;
    [NonSerialized] public bool IsFlashing = false;

    #region First Stage Variables
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

    #region Second Stage Variables
    [SerializeField] private float _timeToSecondStage;
    private float _secondStageTimer;
    #endregion

    #region Third Stage Variables
    [SerializeField] private float _timeToThirdStage;
    private float _thirdStageTimer;
    public GameObject ThirdStageProjectile;
    public GameObject ThirdStageShootAnim;
    public List<Transform> ThirdStageFirstWaveShootDirections = new List<Transform>();
    public List<Transform> ThirdStageSecondWaveShootDirections = new List<Transform>();
    #endregion

    #region Fourth Stage Variables
    [SerializeField] private float _timeToFourthStage;
    private float _fourthStageTimer;
    public Transform FourthStageMovePointLeft;
    public Transform FourthStageMovePointRight;
    public Transform FourthStageAimPointUp;
    public Transform FourthStageAimPointDown;
    public GameObject FourthStageSatellite;
    public Transform FourthStageSatelliteUpPoint;
    public Transform FourthStageSatelliteDownPoint;
    #endregion

    #region Stage States Variables
    [NonSerialized] public AfroditeBaseState CurrentState;
    [NonSerialized] public AfroditeIdleState IdleState = new AfroditeIdleState();
    [NonSerialized] public AfroditeDeathState DeathState = new AfroditeDeathState();
    [NonSerialized] public AfroditeSecondStageState SecondStageState = new AfroditeSecondStageState();
    [NonSerialized] public AfroditeFirstStageState FirstStageState = new AfroditeFirstStageState();
    [NonSerialized] public AfroditeThirdStageState ThirdStageState = new AfroditeThirdStageState();
    [NonSerialized] public AfroditeFourthStageState FourthStageState = new AfroditeFourthStageState();
    #endregion

    public UnityEvent ScreenShakeEvent;

    protected override void Awake()
    {
        base.Awake();
        CurrentState = IdleState;
        CurrentState.EnterState(this);

        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.gameObject.AddComponent<AfroditeResetColor>();
    }

    private void Update()
    {
        // Debug.Log(CurrentState);
        Debug.Log(Health);

        IsFlashing = _flashHitEffect.IsFlashing;
        CurrentState.UpdateState(this);

        if (Health <= 20 && Health > 0)
        {
            if (CurrentState != SecondStageState && CurrentState != ThirdStageState && CurrentState != FourthStageState)
            {
                _secondStageTimer -= Time.deltaTime;
                if (_secondStageTimer <= 0f)
                {
                    SwitchState(SecondStageState);
                    _secondStageTimer = _timeToSecondStage;
                }

                _thirdStageTimer -= Time.deltaTime;
                if (_thirdStageTimer <= 0f)
                {
                    SwitchState(ThirdStageState);
                    _thirdStageTimer = _timeToThirdStage;
                }

                _fourthStageTimer -= Time.deltaTime;
                if (_fourthStageTimer <= 0f)
                {
                    SwitchState(FourthStageState);
                    _fourthStageTimer = _timeToFourthStage;
                }
            }
        }

        if (Health <= 40 && Health > 20)
        {
            if (CurrentState != SecondStageState && CurrentState != ThirdStageState)
            {
                _secondStageTimer -= Time.deltaTime;
                if (_secondStageTimer <= 0f)
                {
                    SwitchState(SecondStageState);
                    _secondStageTimer = _timeToSecondStage;
                }

                _thirdStageTimer -= Time.deltaTime;
                if (_thirdStageTimer <= 0f)
                {
                    SwitchState(ThirdStageState);
                    _thirdStageTimer = _timeToThirdStage;
                }
            }
        }

        if (Health <= 60 && Health > 40)
        {
            _secondStageTimer -= Time.deltaTime;
            if (_secondStageTimer <= 0f && CurrentState != SecondStageState)
            {
                SwitchState(SecondStageState);
                _secondStageTimer = _timeToSecondStage;
            }
        }

        if (Health <= 0 && CurrentState != DeathState)
        {
            SwitchState(DeathState);
        }
    }

    public void SwitchState(AfroditeBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CurrentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CurrentState.OnTriggerExit(this, other);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _flashHitEffect.Flash();
    }
}