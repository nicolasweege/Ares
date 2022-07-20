using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeBulletController : BulletBase
{
    private Rigidbody2D _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _destroyVisibleBulletTimer = _timeToDestroyVisibleBullet;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveBullet();
        DeactiveBullet();
        // DestroyVisibleBullet();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Satellite"))
            DestroyBullet();

        if (other.CompareTag("ArenaCollider"))
            DestroyBullet();
    }

    protected override void MoveBullet()
    {
        Vector2 bulletDir = PlayerMainShipController.Instance.transform.position - transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        _direction = new Vector3(bulletDir.x, bulletDir.y);
        transform.position += _direction * Time.deltaTime * _speed;
    }
}