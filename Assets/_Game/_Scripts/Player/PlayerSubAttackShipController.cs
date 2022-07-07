using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerSubAttackShipController : Singleton<PlayerSubAttackShipController>
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletStartingPos;
    [SerializeField] private Transform _bulletDir;
    [SerializeField] private GameObject _deathAnim;
    [SerializeField] private ParticleSystem _turbineFlame;
    [SerializeField] private float _timeToShoot;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _cameraOrthoSize;
    private float _shootTimer;
    private Camera _camera;
    private PlayerInputActions _playerInputActions;

    protected override void Awake()
    {
        base.Awake();
        _camera = FindObjectOfType<Camera>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.SubAttackShip.Enable();

        _turbineFlame.Stop();
    }

    private void Update()
    {
        Move();
        Aim();

        if (_playerInputActions.SubAttackShip.ShootHolding.IsPressed())
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0f)
            {
                GenerateBullet();
                _shootTimer = _timeToShoot;
            }
        }

        var move = _playerInputActions.SubAttackShip.Movement.ReadValue<Vector2>();
        move.Normalize();

        if (_playerInputActions.SubAttackShip.Movement.IsPressed())
        {
            _turbineFlame.Play();
        }
        else
        {
            _turbineFlame.Stop();
        }

        if (_health <= 0)
            Death();

        if (_playerInputActions.SubAttackShip.ChangeToMainShip.IsPressed())
        {
            ChangeToPlayerMainShip();
            PlayerMainShipController.Instance.IsPlayerInSubShip = false;
        }
    }

    private int TakeDamage(int damage) => _health -= damage;

    private void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }

    private void GenerateBullet()
    {
        var bulletInst = Instantiate(_bulletPrefab, _bulletStartingPos.position, _bulletStartingPos.rotation);
        Vector2 bulletDir = _bulletDir.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle - 90f);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }

    private void Move()
    {
        Vector2 moveVector = _playerInputActions.SubAttackShip.Movement.ReadValue<Vector2>();
        moveVector.Normalize();
        transform.position += new Vector3(moveVector.x, moveVector.y) * Time.deltaTime * _speed;
        float xx = Mathf.Clamp(transform.position.x, -LevelManager.Instance.MapWidth, LevelManager.Instance.MapWidth);
        float yy = Mathf.Clamp(transform.position.y, -LevelManager.Instance.MapHight, LevelManager.Instance.MapHight);
        transform.position = new Vector3(xx, yy);
    }

    private Vector2 Aim()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - new Vector2(transform.position.x, transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, lookAngle), _turnSpeed * Time.deltaTime);
        return lookDir;
    }

    private void ChangeToPlayerMainShip()
    {
        CinemachineManager.Instance.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = _cameraOrthoSize;
        CinemachineManager.Instance.GetComponent<CinemachineVirtualCamera>().Follow = PlayerMainShipController.Instance.transform;
        _playerInputActions.SubAttackShip.Disable();
        PlayerMainShipController.Instance.PlayerInputActions.MainShip.Enable();
        PlayerMainShipController.Instance.EnableAimLaser();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
            other.GetComponent<BulletBase>().DestroyBullet();
        }
    }
}