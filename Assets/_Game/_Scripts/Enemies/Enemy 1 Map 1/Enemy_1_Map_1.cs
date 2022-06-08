using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Map_1 : EnemyBase
{
    private Rigidbody2D _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isPlayerInRadar)
        {
            AimAtPlayer();
            Shoot();
            FollowPlayer();
        }

        if (_health <= 0)
            Death();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _isPlayerInRadar = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _isPlayerInRadar = false;
    }
}