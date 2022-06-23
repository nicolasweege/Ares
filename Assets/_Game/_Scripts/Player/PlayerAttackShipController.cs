using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerAttackShipController : Singleton<PlayerAttackShipController>
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _defaultDamage;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletStartingPos;
    [SerializeField] private Transform _bulletDir;
    [SerializeField] private GameObject _deathAnim;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _timeToShoot;
    [SerializeField] private ParticleSystem _rightTurbineFlame;
    [SerializeField] private ParticleSystem _leftTurbineFlame;
    [SerializeField] private Transform _turretTransform;
    #region Stabilizer Trails
    [SerializeField] private ParticleSystem _frontStabilizerTrail;
    [SerializeField] private ParticleSystem _backStabilizerTrail;
    [SerializeField] private ParticleSystem _rightStabilizerTrail;
    [SerializeField] private ParticleSystem _leftStabilizerTrail;
    [SerializeField] private ParticleSystem _rightFrontDStabilizerTrail;
    [SerializeField] private ParticleSystem _leftFrontDStabilizerTrail;
    [SerializeField] private ParticleSystem _rightBackDStabilizerTrail;
    [SerializeField] private ParticleSystem _leftBackDStabilizerTrail;
    #endregion
    private float _shootTimer;
    private Camera _camera;
    private PlayerInputActions _playerInputActions;

    public PlayerInputActions PlayerInputActions { get => _playerInputActions; set => _playerInputActions = value; }

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.MainShip.Disable();
        _playerInputActions.Player.Shoot.performed += Shoot_performed;

        _rightTurbineFlame.startLifetime = 0f;
        _leftTurbineFlame.startLifetime = 0f;
        ResetStabilizers();
    }

    private void Update()
    {
        Move();
        TurretAim();
        HandleStabilizers();

        if (_playerInputActions.Player.Movement.ReadValue<Vector2>().y == 1f)
        {
            _rightTurbineFlame.startLifetime = 0.2f;
            _leftTurbineFlame.startLifetime = 0.2f;
        }
        else
        {
            _rightTurbineFlame.startLifetime = 0f;
            _leftTurbineFlame.startLifetime = 0f;
        }

        if (_playerInputActions.Player.ShootHolding.IsPressed())
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0f)
            {
                GenerateBullet();
                _shootTimer = _timeToShoot;
            }
        }

        if (_health <= 0)
            Death();
    }

    private void ResetStabilizers()
    {
        _frontStabilizerTrail.startLifetime = 0f;
        _backStabilizerTrail.startLifetime = 0f;
        _rightStabilizerTrail.startLifetime = 0f;
        _leftStabilizerTrail.startLifetime = 0f;
        _rightFrontDStabilizerTrail.startLifetime = 0f;
        _leftFrontDStabilizerTrail.startLifetime = 0f;
        _rightBackDStabilizerTrail.startLifetime = 0f;
        _leftBackDStabilizerTrail.startLifetime = 0f;
    }

    private void HandleStabilizers()
    {
        var playerMovement = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        playerMovement.Normalize();

        if (playerMovement.x == 0f && playerMovement.y == -1f)
        {
            ResetStabilizers();
            _frontStabilizerTrail.startLifetime = 0.15f;
        }
        if (playerMovement.x == 0f && playerMovement.y == 1f)
        {
            ResetStabilizers();
            _backStabilizerTrail.startLifetime = 0.15f;
        }
        if (playerMovement.x == 1f && playerMovement.y == 0f)
        {
            ResetStabilizers();
            _leftStabilizerTrail.startLifetime = 0.15f;
        }
        if (playerMovement.x == -1f && playerMovement.y == 0f)
        {
            ResetStabilizers();
            _rightStabilizerTrail.startLifetime = 0.15f;
        }
        if (playerMovement.x > 0f && playerMovement.y > 0f)
        {
            ResetStabilizers();
            _leftBackDStabilizerTrail.startLifetime = 0.15f;
        }
        if (playerMovement.x < 0f && playerMovement.y > 0f)
        {
            ResetStabilizers();
            _rightBackDStabilizerTrail.startLifetime = 0.15f;
        }
        if (playerMovement.x > 0f && playerMovement.y < 0f)
        {
            ResetStabilizers();
            _leftFrontDStabilizerTrail.startLifetime = 0.15f;
        }
        if (playerMovement.x < 0f && playerMovement.y < 0f)
        {
            ResetStabilizers();
            _rightFrontDStabilizerTrail.startLifetime = 0.15f;
        }
    }

    private int TakeDamage(int damage) => _health -= damage;

    private void Shoot_performed(InputAction.CallbackContext context)
    {
        GenerateBullet();
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

    private Vector2 TurretAim()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - new Vector2(transform.position.x, transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        float turnSpeed = 5;
        _turretTransform.rotation = Quaternion.Slerp(_turretTransform.rotation, Quaternion.Euler(0, 0, lookAngle), turnSpeed * Time.deltaTime);
        return lookDir;
    }

    private void Move()
    {
        Vector2 moveVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        moveVector.Normalize();
        transform.position += new Vector3(moveVector.x, moveVector.y) * Time.deltaTime * _speed;
        float xx = Mathf.Clamp(transform.position.x, -LevelManager.Instance.MapWidth, LevelManager.Instance.MapWidth);
        float yy = Mathf.Clamp(transform.position.y, -LevelManager.Instance.MapHight, LevelManager.Instance.MapHight);
        transform.position = new Vector3(xx, yy);
    }

    private void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
            other.GetComponent<BulletBase>().DestroyBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(other.gameObject.GetComponent<EnemyBase>().DefaultDamage);
            other.gameObject.GetComponent<EnemyBase>().Death();
        }
    }
}