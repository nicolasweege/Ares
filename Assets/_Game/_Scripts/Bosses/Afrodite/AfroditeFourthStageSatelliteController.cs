using System;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageSatelliteController : Singleton<AfroditeFourthStageSatelliteController>
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _deathAnim;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private FlashHitEffect _flashHitEffect;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _timeToShoot;
    [SerializeField] private List<Transform> _shootDirections = new List<Transform>();
    private float _shootTimer;

    [NonSerialized] public bool IsFlashing = false;

    protected override void Awake()
    {
        base.Awake();

        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.gameObject.AddComponent<AfroditeFourthStageSatelliteResetColor>();
    }

    private void Update()
    {
        IsFlashing = _flashHitEffect.IsFlashing;

        HandleAttack();

        if (_health <= 0)
        {
            HandleDeath();
        }
    }

    private void TakeDamage(int damage)
    {
        _health -= damage;
        _flashHitEffect.Flash();
    }

    public void HandleDeath()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }

    private void HandleAttack()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            for (int i = 0; i < _shootDirections.Count; i++)
            {
                GenerateBullet(transform, _projectile, _shootDirections[i]);
            }
            _shootTimer = _timeToShoot;
        }
    }

    private void GenerateBullet(Transform bulletStartingPos, GameObject bulletPrefab, Transform projectileDir)
    {
        var bulletInst = Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        Vector2 bulletDir = projectileDir.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
            other.GetComponent<BulletBase>().DestroyBullet();
        }
    }
}