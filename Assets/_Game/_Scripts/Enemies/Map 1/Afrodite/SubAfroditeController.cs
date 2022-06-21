using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAfroditeController : EnemyBase
{
    [SerializeField] private GameObject _mainEnemyPrefab;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletStartingPos;
    [SerializeField] private float _timeToShoot;
    private float _shootTimer;

    private void Update()
    {
        if (_mainEnemyPrefab.GetComponent<AfroditeController>().IsPlayerInRadar)
        {
            AimAtPlayer();
            Shoot();
        }

        if (_health <= 0)
            Death();
    }

    private void GenerateBullet()
    {
        if (PlayerAttackShipController.Instance == null)
            return;

        GameObject bulletInst = Instantiate(_bulletPrefab, _bulletStartingPos.position, Quaternion.identity);
        Vector2 bulletDir = PlayerAttackShipController.Instance.transform.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle - 90f);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }

    private void Shoot()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            GenerateBullet();
            _shootTimer = _timeToShoot;
        }
    }
}