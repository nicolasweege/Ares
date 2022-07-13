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

    public AfroditeBaseState CurrentState;
    public AfroditeIdleState IdleState = new AfroditeIdleState();

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

        transform.position = Vector2.SmoothDamp(transform.position, new Vector2(10f, 0f), ref _velocity, _speed);
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