using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAfroditeController : EnemyBase
{
    [SerializeField] private GameObject _mainEnemyPrefab;
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private Transform _shotStartPos;
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

    private void GenerateShot()
    {
        if (PlayerController.Instance == null)
            return;

        GameObject shotInst = Instantiate(_shotPrefab, _shotStartPos.position, Quaternion.identity);
        Vector2 shotDir = PlayerController.Instance.transform.position - shotInst.transform.position;
        shotDir.Normalize();
        float shotAngle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        shotInst.transform.rotation = Quaternion.Euler(0f, 0f, shotAngle - 90f);
        shotInst.GetComponent<BulletBase>().Direction = new Vector3(shotDir.x, shotDir.y);
    }

    private void Shoot()
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
}