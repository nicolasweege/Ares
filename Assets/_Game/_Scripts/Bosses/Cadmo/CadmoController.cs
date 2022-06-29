using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadmoController : EnemyBase
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletStartingPosUp;
    [SerializeField] private Transform _bulletStartingPosDown;
    [SerializeField] private Transform _bulletStartingPosRight;
    [SerializeField] private Transform _bulletStartingPosLeft;
    [SerializeField] private Transform _bulletDirUp;
    [SerializeField] private Transform _bulletDirDown;
    [SerializeField] private Transform _bulletDirRight;
    [SerializeField] private Transform _bulletDirLeft;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _timeToShoot;
    private float _shootTimer;
    private bool _isPlayerInRadar = false;
    private BoxCollider2D _boxCollider;

    protected override void Awake()
    {
        base.Awake();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);

        if (_isPlayerInRadar)
            Shoot();

        if (_health <= 0)
            Death();
    }

    private void GenerateBullet(Transform bulletStartingPos, Transform bulletDirTo)
    {
        if (PlayerMainShipController.Instance == null)
            return;

        GameObject bulletInst = Instantiate(_bulletPrefab, bulletStartingPos.position, Quaternion.identity);
        Vector2 bulletDir = bulletDirTo.position - bulletInst.transform.position;
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
            GenerateBullet(_bulletStartingPosUp, _bulletDirUp);
            GenerateBullet(_bulletStartingPosLeft, _bulletDirLeft);
            GenerateBullet(_bulletStartingPosRight, _bulletDirRight);
            GenerateBullet(_bulletStartingPosDown, _bulletDirDown);
            _shootTimer = _timeToShoot;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip"))
        {
            _isPlayerInRadar = true;
            _rotation.z = 120f;
            _boxCollider.size = new Vector2(17f, 17f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip"))
        {
            _isPlayerInRadar = false;
            _rotation.z = 40f;
            _boxCollider.size = new Vector2(10f, 10f);
        }
    }
}