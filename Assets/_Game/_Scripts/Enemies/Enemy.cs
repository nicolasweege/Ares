using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected float _speed;
    [SerializeField] protected int _amountOfCoins;
    [SerializeField] protected int _defaultDamage;
    protected float _shootTimer;
    [SerializeField] protected float _timeToShoot;
    [SerializeField] protected float _stoppingDistance;
    [SerializeField] protected GameObject _shotPf;
    [SerializeField] protected Transform _shotStartPos;
    [SerializeField] protected GameObject _deathAnim;
    [SerializeField] protected GameObject _mapGeneratorPf;
    protected Player _playerScript;

    public int DefaultDamage { get => _defaultDamage; set => _defaultDamage = value; }

    public virtual int LoseLife(int damage) => _health -= damage;

    public virtual void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }

    public virtual void CreateShot()
    {
        var player = FindObjectOfType<Player>();
        if (player == null)
            return;

        GameObject shot = Instantiate(_shotPf, _shotStartPos.position, Quaternion.identity);

        Vector2 shotDirection = player.transform.position - shot.transform.position;
        shotDirection.Normalize();

        float shotAngle = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;

        shot.transform.rotation = Quaternion.Euler(0f, 0f, shotAngle + 90f);
        shot.GetComponent<Rigidbody2D>().velocity = shotDirection * shot.GetComponent<Shot>().Speed;
    }

    public virtual void Shoot()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        _shootTimer -= Time.deltaTime;

        if (_shootTimer <= 0)
        {
            CreateShot();
            _shootTimer = _timeToShoot;
        }
    }

    public virtual Vector2 AimAtPlayer()
    {
        Vector2 playerPos = _playerScript.transform.position;
        Vector2 lookDir = playerPos - new Vector2(transform.position.x, transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        return lookDir;
    }

    public virtual void FollowPlayer()
    {
        Vector2 playerPos = _playerScript.transform.position;
        if (Vector2.Distance(transform.position, playerPos) >= _stoppingDistance)
            transform.position = Vector2.MoveTowards(transform.position, playerPos, _speed * Time.deltaTime);
    }
}