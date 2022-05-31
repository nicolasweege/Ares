using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [SerializeField] private float _yLimit;
    [SerializeField] private bool _canMoveToDiagonal;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb = GetComponentInChildren<Rigidbody2D>();
    }

    private void Update()
    {
        Moving();
        Shoot();

        if (_health <= 0)
            Death();
    }

    public void Moving()
    {
        if (transform.position.y > _yLimit || !_canMoveToDiagonal)
            return;
        if (transform.position.x > 0f)
            _rb.velocity = new Vector2(-_speed, -_speed);
        if (transform.position.x < 0f)
            _rb.velocity = new Vector2(_speed, -_speed);
        _canMoveToDiagonal = false;
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