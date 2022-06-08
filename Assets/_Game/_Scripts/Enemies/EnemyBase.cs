using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : StaticInstance<EnemyBase>
{
    [SerializeField] protected int _health;
    [SerializeField] protected float _speed;
    [SerializeField] protected int _amountOfCoins;
    [SerializeField] protected int _defaultDamage;
    [SerializeField] protected float _timeToShoot;
    [SerializeField] protected float _stoppingDist;
    [SerializeField] protected GameObject _shotPrefab;
    [SerializeField] protected Transform _shotStartPos;
    [SerializeField] protected GameObject _deathAnim;
    protected float _shootTimer;
    protected bool _isPlayerInRadar = false;

    public int DefaultDamage { get => _defaultDamage; set => _defaultDamage = value; }

    public virtual int TakeDamage(int damage) => _health -= damage;

    public virtual void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }

    public virtual void GenerateShot()
    {
        if (PlayerController.Instance == null)
            return;

        GameObject shotInst = Instantiate(_shotPrefab, _shotStartPos.position, Quaternion.identity);
        Vector2 shotDir = PlayerController.Instance.transform.position - shotInst.transform.position;
        shotDir.Normalize();
        float shotAngle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        shotInst.transform.rotation = Quaternion.Euler(0f, 0f, shotAngle + 90f);
        shotInst.GetComponent<Rigidbody2D>().velocity = shotDir * shotInst.GetComponent<ShotBase>().Speed;
    }

    public virtual void Shoot()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            GenerateShot();
            _shootTimer = _timeToShoot;
        }
    }

    public virtual Vector2 AimAtPlayer()
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(transform.position.x, transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        return lookDir;
    }

    public virtual void FollowPlayer()
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;
        if (Vector2.Distance(transform.position, playerPos) >= _stoppingDist)
            transform.position = Vector2.MoveTowards(transform.position, playerPos, _speed * Time.deltaTime);
    }
}