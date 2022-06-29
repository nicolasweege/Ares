using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMainShipController : Singleton<PlayerMainShipController>
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
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private GameObject _subAttackShipPrefab;
    private float _shootTimer;
    private Camera _camera;
    private bool _isPlayerInSubShip = false;
    private PlayerInputActions _playerInputActions;
    #region Stabilizer Trails/Turbine Flames - Particles
    [SerializeField] private ParticleSystem _rightTurbineFlame;
    [SerializeField] private ParticleSystem _leftTurbineFlame;
    [SerializeField] private ParticleSystem _frontStabilizerTrail;
    [SerializeField] private ParticleSystem _backStabilizerTrail;
    [SerializeField] private ParticleSystem _rightStabilizerTrail;
    [SerializeField] private ParticleSystem _leftStabilizerTrail;
    [SerializeField] private ParticleSystem _rightFrontDStabilizerTrail;
    [SerializeField] private ParticleSystem _leftFrontDStabilizerTrail;
    [SerializeField] private ParticleSystem _rightBackDStabilizerTrail;
    [SerializeField] private ParticleSystem _leftBackDStabilizerTrail;
    #endregion

    public bool IsPlayerInSubShip { get => _isPlayerInSubShip; set => _isPlayerInSubShip = value; }
    public PlayerInputActions PlayerInputActions { get => _playerInputActions; set => _playerInputActions = value; }

    protected override void Awake()
    {
        base.Awake();
        _camera = FindObjectOfType<Camera>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.MainShip.Enable();

        ResetStabilizers();
        ResetTurbineFlames();
    }

    private void Update()
    {
        HandleStabilizers();

        var move = _playerInputActions.MainShip.Movement.ReadValue<Vector2>();
        move.Normalize();

        if (Mathf.Round(move.y) == 1f)
        {
            _rightTurbineFlame.Play();
            _leftTurbineFlame.Play();
        }
        else
        {
            ResetTurbineFlames();
        }

        if (!_isPlayerInSubShip)
        {
            Move();
            TurretAim();

            if (_playerInputActions.MainShip.ShootHolding.IsPressed())
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

        if (_playerInputActions.MainShip.ChangeToSubAttackShip.IsPressed() && !_isPlayerInSubShip)
        {
            ChangeToSubAttackShip();
            _isPlayerInSubShip = true;
        }
    }

    private void ResetTurbineFlames()
    {
        _rightTurbineFlame.Stop();
        _leftTurbineFlame.Stop();
    }

    private void ResetStabilizers()
    {
        _frontStabilizerTrail.Stop();
        _backStabilizerTrail.Stop();
        _rightStabilizerTrail.Stop();
        _leftStabilizerTrail.Stop();
        _rightFrontDStabilizerTrail.Stop();
        _leftFrontDStabilizerTrail.Stop();
        _rightBackDStabilizerTrail.Stop();
        _leftBackDStabilizerTrail.Stop();
    }

    private void HandleStabilizers()
    {
        var move = _playerInputActions.MainShip.Movement.ReadValue<Vector2>();
        move.Normalize();

        // Moving to Back
        if (Mathf.Round(move.x) == 0f && Mathf.Round(move.y) == -1f)
        {
            ResetStabilizers();
            if (!_frontStabilizerTrail.isEmitting)
                _frontStabilizerTrail.Play();
        }
        // Moving to Front
        if (Mathf.Round(move.x) == 0f && Mathf.Round(move.y) == 1f)
        {
            ResetStabilizers();
            _backStabilizerTrail.Play();
        }
        // Moving to Right
        if (Mathf.Round(move.x) == 1f && Mathf.Round(move.y) == 0f)
        {
            ResetStabilizers();
            _leftStabilizerTrail.Play();
        }
        // Moving to Left
        if (Mathf.Round(move.x) == -1f && Mathf.Round(move.y) == 0f)
        {
            ResetStabilizers();
            _rightStabilizerTrail.Play();
        }
        // Moving to Diagonal Front/Right
        if (Mathf.Round(move.x) > 0f && Mathf.Round(move.y) > 0f)
        {
            ResetStabilizers();
            _leftBackDStabilizerTrail.Play();
        }
        // Moving to Diagonal Front/Left
        if (Mathf.Round(move.x) < 0f && Mathf.Round(move.y) > 0f)
        {
            ResetStabilizers();
            _rightBackDStabilizerTrail.Play();
        }
        // Moving to Diagonal Back/Right
        if (Mathf.Round(move.x) > 0f && Mathf.Round(move.y) < 0f)
        {
            ResetStabilizers();
            _leftFrontDStabilizerTrail.Play();
        }
        // Moving to Diagonal Back/Left
        if (Mathf.Round(move.x) < 0f && Mathf.Round(move.y) < 0f)
        {
            ResetStabilizers();
            _rightFrontDStabilizerTrail.Play();
        }

        if (Mathf.Round(move.x) == 0f && Mathf.Round(move.y) == 0f)
        {
            ResetStabilizers();
        }
    }

    public int TakeDamage(int damage) => _health -= damage;

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
        _turretTransform.rotation = Quaternion.Slerp(_turretTransform.rotation, Quaternion.Euler(0, 0, lookAngle), _turnSpeed * Time.deltaTime);
        return lookDir;
    }

    private void Move()
    {
        Vector2 moveVector = _playerInputActions.MainShip.Movement.ReadValue<Vector2>();
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

    private void ChangeToSubAttackShip()
    {
        var subAttackShipInst = Instantiate(_subAttackShipPrefab, new Vector3(transform.position.x, transform.position.y - 3f), Quaternion.identity);
        CinemachineManager.Instance.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 6f;
        CinemachineManager.Instance.GetComponent<CinemachineVirtualCamera>().Follow = subAttackShipInst.transform;
        _playerInputActions.MainShip.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
            other.GetComponent<BulletBase>().DestroyBullet();
        }

        if (other.CompareTag("PlayerSubAttackShipBullet"))
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