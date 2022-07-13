using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeController : EnemyBase
{
    [SerializeField] private Vector3 _rotation;
    public bool IsPlayerInRadar = false;
    public BoxCollider2D BoxCollider;
    private Vector2 _velocity = Vector2.zero;
    [Range(0f, 100f)]
    public float TurnSpeed;
    public GameObject LaserBeam;

    public AfroditeBaseState CurrentState;
    public AfroditeIdleState IdleState = new AfroditeIdleState();
    public AfroditeLaserShootState LaserShootState = new AfroditeLaserShootState();
    public AfroditeAimingState AimingState = new AfroditeAimingState();

    protected override void Awake()
    {
        base.Awake();
        BoxCollider = GetComponentInChildren<BoxCollider2D>();
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState.UpdateState(this);

        if (_health <= 0)
            Death();

        Debug.Log(CurrentState);

        transform.position = Vector2.SmoothDamp(transform.position, new Vector2(5f, 0f), ref _velocity, _speed);
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
}