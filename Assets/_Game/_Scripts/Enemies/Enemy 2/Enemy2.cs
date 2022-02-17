using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
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

    private void CreateShot()
    {
        var player = FindObjectOfType<Player>();
        if (player == null) return;

        GameObject shot = Instantiate(_shot, _shotStartPosition.position, _shotStartPosition.rotation);

        Vector2 shotDirection = player.transform.position - shot.transform.position;
        shotDirection.Normalize();

        float shotAngle = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;

        shot.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, shotAngle + 90f));
        shot.GetComponent<Rigidbody2D>().velocity = shotDirection * shot.GetComponent<Shot>().Speed;
    }

    private void Shoot()
    {
        bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;

        if (enemyIsVisible)
        {
            _timeToShoot -= Time.deltaTime;

            if (_timeToShoot <= 0)
            {
                CreateShot();
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
