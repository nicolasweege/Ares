using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeBulletController : BulletBase
{
    [SerializeField] private float _timeToRecover;
    [SerializeField] private Transform _damageAnimSpawnPoint;
    private float _recoverTimer;
    private Rigidbody2D _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _destroyVisibleBulletTimer = _timeToDestroyVisibleBullet;
        _rigidbody = GetComponent<Rigidbody2D>();
        _recoverTimer = _timeToRecover;
    }

    private void Update()
    {
        _recoverTimer -= Time.deltaTime;
        if (_recoverTimer <= 0f)
        {
            RecoverProjectile();
            // _recoverTimer = _timeToRecover;
        }
        else
        {
            MoveProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Satellite"))
            DestroyBullet();

        if (other.CompareTag("ArenaCollider"))
            DestroyBullet();
    }

    public override void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(_damageAnim, _damageAnimSpawnPoint.position, Quaternion.identity);
    }

    private void MoveProjectile()
    {
        Vector2 bulletDir = PlayerMainShipController.Instance.transform.position - transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        _direction = new Vector3(bulletDir.x, bulletDir.y);
        transform.position += _direction * Time.deltaTime * _speed;
    }

    private void RecoverProjectile()
    {
        // _direction = AfroditeController.Instance.FirstStageProjectileDir;
        transform.position += _direction * Time.deltaTime * _speed;
    }
}