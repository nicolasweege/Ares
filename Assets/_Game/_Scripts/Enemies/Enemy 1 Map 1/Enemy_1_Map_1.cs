using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Map_1 : Enemy
{
    [SerializeField] protected GameObject _playerPf;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerScript = FindObjectOfType<Player>();
    }

    private void Update()
    {
        AimAtPlayer();
        Shoot();
        FollowPlayer();

        if (_health <= 0)
            Death();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Shot"))
        {
            bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
            if (!isEnemyVisible)
                return;

            LoseLife(other.GetComponent<Shot>().DefaultDamage);
            other.GetComponent<Shot>().DestroyShot();
        }
    }
}