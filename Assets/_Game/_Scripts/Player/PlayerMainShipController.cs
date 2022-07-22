using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Events;

public class PlayerMainShipController : Singleton<PlayerMainShipController>
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _defaultDamage;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletStartingPos;
    [SerializeField] private Transform _bulletDir;
    [SerializeField] private GameObject _deathAnim;
    [SerializeField] private float _timeToShoot;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _laserBeam;
    [SerializeField] private ParticleSystem _turbineFlame;
    [SerializeField] private float _dashAmount;
    [SerializeField] private LayerMask _dashRaycastLayerMask;
    [SerializeField] private Transform _aimTransform;
    private bool _isShieldEnabled = false;
    private float _shootTimer;
    private Camera _camera;
    private PlayerInputActions _playerInputActions;
    private Rigidbody2D _rigidbody;
    private bool _dashing = false;
    private Vector2 _moveVector;

    [SerializeField] private UnityEvent _screenShakeEvent;

    public PlayerInputActions PlayerInputActions { get => _playerInputActions; set => _playerInputActions = value; }

    protected override void Awake()
    {
        base.Awake();
        _camera = FindObjectOfType<Camera>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.MainShip.Enable();
        _shield.SetActive(false);
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 mousePos = Utils.GetMouseWorldPosition();
        _aimTransform.LookAt(mousePos);

        HandleMove();
        HandleAim();
        HandleTurbineFlame();
        HandleShield();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _dashing = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _laserBeam.GetComponent<PlayerLaserBeamController>().EnableLaser();
        }
        if (Input.GetMouseButton(1))
        {
            _laserBeam.GetComponent<PlayerLaserBeamController>().UpdateLaser();
        }
        if (Input.GetMouseButtonUp(1))
        {
            _laserBeam.GetComponent<PlayerLaserBeamController>().DisableLaser();
        }

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

    public int TakeDamage(int damage) => _health -= damage;

    private void HandleTurbineFlame()
    {
        if (_playerInputActions.MainShip.Movement.IsPressed())
        {
            _turbineFlame.Play();
        }
        else
        {
            _turbineFlame.Stop();
        }
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

    private Vector2 HandleAim()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - new Vector2(transform.position.x, transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, lookAngle), _turnSpeed * Time.deltaTime);
        return lookDir;
    }

    private void HandleMove()
    {
        _moveVector = _playerInputActions.MainShip.Movement.ReadValue<Vector2>().normalized;
        transform.position += new Vector3(_moveVector.x, _moveVector.y) * Time.deltaTime * _speed;

        // _rigidbody.velocity = new Vector3(moveVector.x, moveVector.y) * _speed;

        /*float xx = Mathf.Clamp(transform.position.x, -LevelManager.Instance.MapWidth, LevelManager.Instance.MapWidth);
        float yy = Mathf.Clamp(transform.position.y, -LevelManager.Instance.MapHight, LevelManager.Instance.MapHight);
        transform.position = new Vector3(xx, yy);*/

        if (_dashing)
        {
            var dashPos = transform.position + new Vector3(_moveVector.x, _moveVector.y) * Time.deltaTime * _dashAmount;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveVector, Time.deltaTime * _dashAmount, _dashRaycastLayerMask);
            if (hit.collider != null)
            {
                dashPos = hit.point;
            }

            transform.position = dashPos;
            _dashing = false;
        }
    }

    private void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }

    private void HandleShield()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _shield.SetActive(true);
            _isShieldEnabled = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _shield.SetActive(false);
            _isShieldEnabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (!_isShieldEnabled)
            {
                TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
            }
            other.GetComponent<BulletBase>().DestroyBullet();
            CinemachineManager.Instance.ScreenShakeEvent(_screenShakeEvent);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!_isShieldEnabled)
            {
                TakeDamage(other.gameObject.GetComponent<EnemyBase>().DefaultDamage);
            }
            // other.gameObject.GetComponent<EnemyBase>().Death();
        }

        if (other.gameObject.CompareTag("Satellite"))
            Death();
    }
}