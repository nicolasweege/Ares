using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AfroditeController : Singleton<AfroditeController>
{
    public int Health;
    public float Speed;
    [SerializeField] private GameObject _deathAnim;
    [Range(0f, 100f)]
    public float TurnSpeed;
    public GameObject LaserBeam;
    public Transform MovePointCenter;
    public List<Transform> MovePoints = new List<Transform>();

    #region First Stage Props
    public Vector3 CurrentFirstStageProjectileDir;
    public GameObject FirstStageProjectile;
    public Transform FirstStageProjectileStartingPoint;
    public Transform FirstStageProjectileDir;
    #endregion

    #region Third Stage Props
    [SerializeField] private float _timeToThirdStage;
    private float _thirdStageTimer;
    public GameObject ThirdStageProjectile;
    #endregion

    #region Stage States
    public AfroditeBaseState CurrentState;
    public AfroditeIdleState IdleState = new AfroditeIdleState();
    public AfroditeSecondStageState SecondStageState = new AfroditeSecondStageState();
    public AfroditeFirstStageState FirstStageState = new AfroditeFirstStageState();
    public AfroditeThirdStageState ThirdStageState = new AfroditeThirdStageState();
    #endregion

    public UnityEvent ScreenShakeEvent;

    protected override void Awake()
    {
        base.Awake();
        CurrentState = IdleState;
        CurrentState.EnterState(this);
        _thirdStageTimer = _timeToThirdStage;
    }

    private void Update()
    {
        CurrentState.UpdateState(this);

        if (CurrentState != ThirdStageState)
        {
            _thirdStageTimer -= Time.deltaTime;
            if (_thirdStageTimer <= 0f)
            {
                SwitchState(ThirdStageState);
                _thirdStageTimer = _timeToThirdStage;
            }
        }

        if (Health <= 0)
            Death();

        Debug.Log(CurrentState);
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

    public int TakeDamage(int damage) => Health -= damage;

    public virtual void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }
}