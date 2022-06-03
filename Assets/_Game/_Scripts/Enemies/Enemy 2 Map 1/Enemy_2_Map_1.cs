using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Map_1 : Enemy
{
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerScript = FindObjectOfType<Player>();
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
        if (other.tag.Equals("Player"))
            _isPlayerInRadar = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
            _isPlayerInRadar = false;
    }
}