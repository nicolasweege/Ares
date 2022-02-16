using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.velocity = new Vector2(0f, -_speed);
    }

    private void Update()
    {
        Shoot();

        if (_health <= 0) Death();
    }

    private void Shoot()
    {
        bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;

        if (enemyIsVisible)
        {
            _timeToShoot -= Time.deltaTime;

            if (_timeToShoot <= 0)
            {
                CreateGenericShot();
                _timeToShoot = Random.Range(_minTimeToShoot, _maxTimeToShoot);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "InstanceDestroyer":
                bool v = transform.position.y <= 0;
                if (v) Destroy(gameObject);
                break;
        }
    }
}
