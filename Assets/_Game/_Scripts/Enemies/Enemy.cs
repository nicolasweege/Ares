using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _timeToShoot;
    [SerializeField] protected float _minTimeToShoot;
    [SerializeField] protected float _maxTimeToShoot;
    [SerializeField] protected GameObject _shot;
    [SerializeField] protected Transform _shotStartPosition;
    [SerializeField] private GameObject _deathAnimation;
    public int DefaultDamage;

    public int LoseLife(int damage)
    {
        return _health -= damage;
    }

    public void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnimation, transform.position, transform.rotation);
    }
}
