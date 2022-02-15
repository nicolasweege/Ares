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

    protected void CreateShot()
    {
        GameObject shot = Instantiate(_shot, _shotStartPosition.position, _shotStartPosition.rotation);
        shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -shot.GetComponent<Shot>().Speed);
    }

    protected void CreateGuidedShot()
    {
        GameObject shot = Instantiate(_shot, _shotStartPosition.position, _shotStartPosition.rotation);
        var player = FindObjectOfType<Player>();
        Vector2 shotDirection = player.transform.position - shot.transform.position;
        shotDirection.Normalize();
        shot.GetComponent<Rigidbody2D>().velocity = shotDirection * shot.GetComponent<Shot>().Speed;
    }

    public void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnimation, transform.position, transform.rotation);
    }

    protected void Shoot()
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

    protected void GuidedShoot()
    {
        bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;

        if (enemyIsVisible)
        {
            _timeToShoot -= Time.deltaTime;

            if (_timeToShoot <= 0)
            {
                CreateGuidedShot();
                _timeToShoot = Random.Range(_minTimeToShoot, _maxTimeToShoot);
            }
        }
    }
}
