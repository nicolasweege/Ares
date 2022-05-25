using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected int _health;
        [SerializeField] protected float _speed;
        [SerializeField] protected int _amountOfCoins;
        [SerializeField] protected int _defaultDamage;
        [SerializeField] protected float _timeToShoot;
        protected float _minTimeToShoot;
        protected float _maxTimeToShoot;
        [SerializeField] protected GameObject _shotPf;
        [SerializeField] protected Transform _shotStartPos;
        [SerializeField] protected GameObject _deathAnim;

        public int Health { get => _health; set => _health = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public int AmountOfCoins { get => _amountOfCoins; set => _amountOfCoins = value; }
        public int DefaultDamage { get => _defaultDamage; set => _defaultDamage = value; }
        public float TimeToShoot { get => _timeToShoot; set => _timeToShoot = value; }
        public float MinTimeToShoot { get => _minTimeToShoot; set => _minTimeToShoot = value; }
        public float MaxTimeToShoot { get => _maxTimeToShoot; set => _maxTimeToShoot = value; }
        public GameObject ShotPf { get => _shotPf; set => _shotPf = value; }
        public Transform ShotStartPos { get => _shotStartPos; set => _shotStartPos = value; }

        #region Shared Components
        public Shoot Shoot { get => GetComponent<Shoot>(); }
        public CreateShot CreateShot { get => GetComponent<CreateShot>(); }
        #endregion

        public virtual int LoseLife(int damage)
        {
            return Health -= damage;
        }

        public virtual void Death()
        {
            Destroy(gameObject);
            Instantiate(_deathAnim, transform.position, Quaternion.identity);
        }
    }
}