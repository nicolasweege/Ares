using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeController : EnemyBase
{
    [SerializeField] private Vector3 _rotation;
    private bool _isPlayerInRadar = false;
    private BoxCollider2D _boxCollider;
    public static event Action<AfroditeState> OnBeforeAfroditeStateChanged;
    public static event Action<AfroditeState> OnAfterAfroditeStateChanged;

    public AfroditeState State { get; private set; }

    public bool IsPlayerInRadar { get => _isPlayerInRadar; set => _isPlayerInRadar = value; }

    protected override void Awake()
    {
        base.Awake();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        if (_isPlayerInRadar)
        {
            transform.Rotate(_rotation * Time.deltaTime);
        }

        if (_health <= 0)
            Death();
    }

    public void UpdateAfroditeState(AfroditeState newState)
    {
        OnBeforeAfroditeStateChanged?.Invoke(newState);

        State = newState;

        switch (newState)
        {
            case AfroditeState.Idle:
                break;
            case AfroditeState.Moving:
                break;
        }

        OnAfterAfroditeStateChanged?.Invoke(newState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip") || other.CompareTag("PlayerSubAttackShip"))
        {
            _isPlayerInRadar = true;
            _boxCollider.size = new Vector2(30f, 30f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip") || other.CompareTag("PlayerSubAttackShip"))
        {
            _isPlayerInRadar = false;
            _boxCollider.size = new Vector2(15f, 15f);
        }
    }
}

[SerializeField] public enum AfroditeState
{
    Idle = 0,
    Moving = 1
}