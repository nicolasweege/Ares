using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : StaticInstance<BulletBase>
{
    [SerializeField] protected float _speed;
    [SerializeField] protected int _defaultDamage;
    [SerializeField] protected GameObject _damageAnim;
    [SerializeField] protected Vector3 _direction;
    protected float _timeToDeactiveShot = 1f;

    public float Speed { get => _speed; set => _speed = value; }
    public int DefaultDamage { get => _defaultDamage; set => _defaultDamage = value; }
    public Vector3 Direction { get => _direction; set => _direction = value; }

    public virtual void DestroyShot()
    {
        Destroy(gameObject);
        Instantiate(_damageAnim, transform.position, Quaternion.identity);
    }

    public virtual void DeactiveShot()
    {
        _timeToDeactiveShot -= Time.deltaTime;
        bool isShotVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isShotVisible && _timeToDeactiveShot <= 0f)
            Destroy(gameObject);
    }

    protected void MoveShot() => transform.position += _direction * Time.deltaTime * _speed;
}