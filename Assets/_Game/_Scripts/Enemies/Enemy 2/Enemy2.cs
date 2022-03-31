using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [SerializeField] private float _yLimit;
    [SerializeField] private bool _canMove2Diagonal;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.velocity = new Vector2(0f, -_speed);
    }

    private void Update()
    {
        Moving();
        Shoot();

        if (_health <= 0) Death();
    }

    private void Moving()
    {
        if (transform.position.y > _yLimit || !_canMove2Diagonal) return;
        if (transform.position.x > 0f) _rigidbody2D.velocity = new Vector2(-_speed, -_speed);
        if (transform.position.x < 0f) _rigidbody2D.velocity = new Vector2(_speed, -_speed);
        _canMove2Diagonal = false;
    }

    private void CreateShot()
    {
        var player = FindObjectOfType<Player>();
        if (player == null) return;

        GameObject shot = Instantiate(_shot, _shotStartPosition.position, Quaternion.identity);

        Vector2 shotDirection = player.transform.position - shot.transform.position;
        shotDirection.Normalize();

        float shotAngle = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;

        shot.transform.rotation = Quaternion.Euler(0f, 0f, shotAngle + 90f);
        shot.GetComponent<Rigidbody2D>().velocity = shotDirection * shot.GetComponent<Shot>().GetSpeed();
    }

    private void Shoot()
    {
        bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!enemyIsVisible) return;

        _timeToShoot -= Time.deltaTime;

        if (_timeToShoot <= 0)
        {
            CreateShot();
            _timeToShoot = Random.Range(_minTimeToShoot, _maxTimeToShoot);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "InstanceDestroyer":
                bool v = transform.position.y <= 0;
                if (v) Destroy(gameObject);
                break;

            case "Shot":
                bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
                if (enemyIsVisible)
                {
                    LoseLife(other.GetComponent<Shot>().GetDefaultDamage());
                    other.GetComponent<Shot>().DestroyShot();
                }
                break;
        }
    }
}