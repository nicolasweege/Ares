using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub_Enemy_1_Map_1 : EnemyBase
{
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private Transform _shotStartPosTop1;
    [SerializeField] private Transform _shotStartPosTop2;
    [SerializeField] private Transform _shotStartPosBottom1;
    [SerializeField] private Transform _shotStartPosBottom2;
    [SerializeField] private Transform _shotStartPosRight;
    [SerializeField] private Transform _shotStartPosLeft;
    [SerializeField] private Transform _shootToTop1;
    [SerializeField] private Transform _shootToTop2;
    [SerializeField] private Transform _shootToBottom1;
    [SerializeField] private Transform _shootToBottom2;
    [SerializeField] private Transform _shootToRight;
    [SerializeField] private Transform _shootToLeft;
    [SerializeField] private float _timeToShoot;
    private float _shootTimer;

    private void Update()
    {
        FollowPlayer();
        Shoot();

        if (_health <= 0)
            Death();
    }

    private void GenerateShot(Transform shotStartPos, Transform shootTo)
    {
        if (PlayerController.Instance == null)
            return;

        GameObject shotInst = Instantiate(_shotPrefab, shotStartPos.position, Quaternion.identity);
        Vector2 shotDir = shootTo.position - shotInst.transform.position;
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
            GenerateShot(_shotStartPosTop1, _shootToTop1);
            GenerateShot(_shotStartPosTop2, _shootToTop2);
            GenerateShot(_shotStartPosBottom1, _shootToBottom1);
            GenerateShot(_shotStartPosBottom2, _shootToBottom2);
            GenerateShot(_shotStartPosRight, _shootToRight);
            GenerateShot(_shotStartPosLeft, _shootToLeft);
            _shootTimer = _timeToShoot;
        }
    }
}