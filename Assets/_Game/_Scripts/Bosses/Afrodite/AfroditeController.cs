using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeController : EnemyBase
{
    [SerializeField] private Vector3 _rotation;
    private bool _isPlayerInRadar = false;
    private BoxCollider2D _boxCollider;

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