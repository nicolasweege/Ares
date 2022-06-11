using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Map_1 : EnemyBase
{
    [SerializeField] private Transform _shotStartPos;
    private Rigidbody2D _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isPlayerInRadar)
        {
            FollowPlayer();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _isPlayerInRadar = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _isPlayerInRadar = false;
    }
}