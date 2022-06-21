using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : StaticInstance<BulletBase>
{
    [SerializeField] protected float _speed;
    [SerializeField] protected int _defaultDamage;
    [SerializeField] protected GameObject _damageAnim;
    [SerializeField] protected float _timeToDestroyVisibleBullet;
    protected Vector3 _direction;
    protected float _destroyVisibleBulletTimer;
    protected float _timeToDeactiveBullet = 1f;

    public float Speed { get => _speed; set => _speed = value; }
    public int DefaultDamage { get => _defaultDamage; set => _defaultDamage = value; }
    public Vector3 Direction { get => _direction; set => _direction = value; }

    public virtual void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(_damageAnim, transform.position, Quaternion.identity);
    }

    public virtual void DeactiveBullet()
    {
        _timeToDeactiveBullet -= Time.deltaTime;
        bool isShotVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isShotVisible && _timeToDeactiveBullet <= 0f)
            Destroy(gameObject);
    }

    protected virtual void DestroyVisibleBullet()
    {
        _destroyVisibleBulletTimer -= Time.deltaTime;
        if (_destroyVisibleBulletTimer <= 0f)
            DestroyBullet();
    }

    protected virtual void MoveBullet() => transform.position += _direction * Time.deltaTime * _speed;
}