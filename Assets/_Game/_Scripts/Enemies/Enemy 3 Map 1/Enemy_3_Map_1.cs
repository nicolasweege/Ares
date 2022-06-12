using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3_Map_1 : EnemyBase
{
    [SerializeField] private GameObject _shotPrefab;
    [SerializeField] private Transform _shotStartPosLeft;
    [SerializeField] private Transform _shotStartPosRight;
    [SerializeField] private Transform _shotStartPosUp;
    [SerializeField] private Transform _shotStartPosDown;
    [SerializeField] private Transform _shootToUp;
    [SerializeField] private Transform _shootToDown;
    [SerializeField] private Transform _shootToRight;
    [SerializeField] private Transform _shootToLeft;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _timeToShoot;
    private float _shootTimer;
    private bool _isPlayerInRadar = false;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
        if (_isPlayerInRadar)
        {
            Shoot();
        }

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
            GenerateShot(_shotStartPosUp, _shootToUp);
            GenerateShot(_shotStartPosLeft, _shootToLeft);
            GenerateShot(_shotStartPosRight, _shootToRight);
            GenerateShot(_shotStartPosDown, _shootToDown);
            _shootTimer = _timeToShoot;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRadar = true;
            _rotation.z = 120f;
            _boxCollider.size = new Vector2(17f, 17f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRadar = false;
            _rotation.z = 40f;
            _boxCollider.size = new Vector2(10f, 10f);
        }
    }
}