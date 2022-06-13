using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Enemy_1_Map_1 : EnemyBase
{
    [SerializeField] private GameObject _mainEnemyPrefab;
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private Transform _shotStartPos;
    [SerializeField] private float _timeToShoot;
    private float _shootTimer;

    private void Update()
    {
        if (_mainEnemyPrefab.GetComponent<Enemy_1_Map_1>().IsPlayerInRadar)
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
        shotInst.transform.rotation = Quaternion.Euler(0f, 0f, shotAngle + 90f);
        shotInst.GetComponent<Rigidbody2D>().velocity = shotDir * shotInst.GetComponent<ShotBase>().Speed;
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