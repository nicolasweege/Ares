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
    [SerializeField] private GameObject _portalPrefab;
    [SerializeField] private ParticleSystem _centerStabilizerTrail;
    [SerializeField] private ParticleSystem _rightStabilizerTrail;
    [SerializeField] private ParticleSystem _leftStabilizerTrail;
    [SerializeField] private ParticleSystem _rightTurbineFlame;
    [SerializeField] private ParticleSystem _leftTurbineFlame;
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

        _centerStabilizerTrail.startLifetime = 0f;
        _rightStabilizerTrail.startLifetime = 0f;
        _leftStabilizerTrail.startLifetime = 0f;
        _rightTurbineFlame.startLifetime = 0f;
        _leftTurbineFlame.startLifetime = 0f;
    }

    private void Update()
    {
        Move();
        Aim();

        if (Input.GetKey(KeyCode.A))
            _rightStabilizerTrail.startLifetime = 0.15f;

        if (Input.GetKey(KeyCode.D))
            _leftStabilizerTrail.startLifetime = 0.15f;

        if (Input.GetKeyUp(KeyCode.A))
            _rightStabilizerTrail.startLifetime = 0f;

        if (Input.GetKeyUp(KeyCode.D))
            _leftStabilizerTrail.startLifetime = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            _centerStabilizerTrail.startLifetime = 0.15f;
            _rightTurbineFlame.startLifetime = 0.2f;
            _leftTurbineFlame.startLifetime = 0.2f;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            _centerStabilizerTrail.startLifetime = 0f;
            _rightTurbineFlame.startLifetime = 0f;
            _leftTurbineFlame.startLifetime = 0f;
        }

        if (_playerInputActions.Player.Movement.ReadValue<Vector2>() != new Vector2(0f, 0f))
        {
            _rightTurbineFlame.startLifetime = 0.2f;
        }
        else
        {
            _rightTurbineFlame.startLifetime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.R))
            Instantiate(_portalPrefab, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f), Quaternion.identity);

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

    private Vector2 Aim()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - new Vector2(transform.position.x, transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
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