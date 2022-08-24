using UnityEngine;

public abstract class BulletBase : StaticInstance<BulletBase>
{
    [SerializeField] protected float _speed;
    [SerializeField] protected int _defaultDamage;
    [SerializeField] protected GameObject _damageAnim;
    protected Vector3 _direction;

    public float Speed { get => _speed; set => _speed = value; }
    public int DefaultDamage { get => _defaultDamage; set => _defaultDamage = value; }
    public Vector3 Direction { get => _direction; set => _direction = value; }

    public virtual void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(_damageAnim, transform.position, Quaternion.identity);
    }

    protected virtual void MoveBullet() {
        transform.position += _direction * Time.deltaTime * _speed;
    }
}