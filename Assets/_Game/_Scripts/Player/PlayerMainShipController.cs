using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class PlayerMainShipController : Singleton<PlayerMainShipController>
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _defaultDamage;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletStartingPos;
    [SerializeField] private Transform _bulletDir;
    [SerializeField] private GameObject _deathAnim;
    [SerializeField] private GameObject _dashAnim;
    [SerializeField] private float _timeToShoot;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _laserBeam;
    [SerializeField] private ParticleSystem _turbineFlame;
    [SerializeField] private float _dashAmount;
    [SerializeField] private LayerMask _dashRaycastLayerMask;
    [SerializeField] private Transform _aimTransform;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _timeToActivateShield;
    [SerializeField] private float _timeToCanTakeDamage;
    [SerializeField] private float _timeToCanMove;
    [SerializeField] private Color _flashColor;
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private Renderer2DData _renderer2DData;

    [SerializeField] private UnityEvent _screenShakeEvent;

    private bool _isShieldEnabled = false;
    private float _shootTimer;
    private float _canMoveTimer;
    private float _canTakeDamageTimer;
    private float _dashCooldownTimer;
    private float _activateShieldTimer;
    private bool _isDashing = false;
    private bool _canActivateShield = false;
    private bool _canMove = true;
    private bool _isFlickerEnabled = false;
    private Camera _camera;
    private PlayerInputActions _playerInputActions;
    [NonSerialized] public bool CanTakeDamage = true;
    [NonSerialized] public bool CanResetColors = true;
    [NonSerialized] public Vector2 MoveVector;

    public PlayerInputActions PlayerInputActions { get => _playerInputActions; set => _playerInputActions = value; }

    protected override void Awake()
    {
        base.Awake();
        _camera = FindObjectOfType<Camera>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.MainShip.Enable();
        _shield.SetActive(false);
        _canTakeDamageTimer = _timeToCanTakeDamage;
        _canMoveTimer = _timeToCanMove;

        Cursor.SetCursor(_cursorTexture, new Vector2(_cursorTexture.width / 2, _cursorTexture.height / 2), CursorMode.ForceSoftware);

        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.gameObject.AddComponent<PlayerResetColor>();
    }

    private void Update()
    {
        HandleMove();
        HandleAim();
        HandleTurbineFlame();
        HandleShield();
        HandleDamange();

        if (_playerInputActions.MainShip.Dash.IsPressed())
        {
            if (_dashCooldownTimer <= 0f)
            {
                Instantiate(_dashAnim, transform.position, Quaternion.identity);
                _isDashing = true;
            }
        }

        _shootTimer -= Time.deltaTime;
        if (_playerInputActions.MainShip.NormalShoot.IsPressed() && _canMove)
        {
            if (_shootTimer <= 0f)
            {
                GenerateBullet();
                SoundManager.PlaySound(SoundManager.Sound.PlayerShoot, transform.position);
                _shootTimer = _timeToShoot;
            }
        }

        if (_health <= 0)
            Death();
    }

    public void TakeDamage(int damage)
    {
        if (CanTakeDamage)
        {
            _health -= damage;
            CinemachineManager.Instance.ScreenShakeEvent(_screenShakeEvent);
            CanTakeDamage = false;
            _canMove = false;
            _isDashing = false;
            _dashCooldownTimer = _dashCooldown;
            _renderer2DData.rendererFeatures[0].SetActive(true);
            _isFlickerEnabled = true;
            StartCoroutine(colorFlickerRoutine());
        }
    }

    IEnumerator colorFlickerRoutine()
    {
        while (_isFlickerEnabled == true)
        {
            CanResetColors = false;
            foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                spr.color = _flashColor;
            _turbineFlame.Stop();
            yield return new WaitForSeconds(0.15f);
            CanResetColors = true;
            _turbineFlame.Play();
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(colorFlickerRoutine());
            _isFlickerEnabled = false;
        }
    }

    public void HandleDamange()
    {
        if (!CanTakeDamage)
        {
            _canTakeDamageTimer -= Time.deltaTime;
            _isFlickerEnabled = true;
        }
        else _isFlickerEnabled = false;

        if (_canTakeDamageTimer <= 0f)
        {
            CanTakeDamage = true;
            _canTakeDamageTimer = _timeToCanTakeDamage;
        }

        if (!_canMove)
        {
            _canMoveTimer -= Time.deltaTime;
            if (_canMoveTimer <= 0f)
            {
                _canMove = true;
                _canMoveTimer = _timeToCanMove;
            }
        }
        else _renderer2DData.rendererFeatures[0].SetActive(false);
    }

    private void HandleTurbineFlame()
    {
        if (_playerInputActions.MainShip.Movement.IsPressed() && CanTakeDamage)
            _turbineFlame.Play();
        else
            _turbineFlame.Stop();
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
        if (_canMove)
        {
            MoveVector = _playerInputActions.MainShip.Movement.ReadValue<Vector2>().normalized;
            transform.position += new Vector3(MoveVector.x, MoveVector.y) * Time.deltaTime * _speed;

            _dashCooldownTimer -= Time.deltaTime;
            if (_isDashing && _dashCooldownTimer <= 0f)
            {
                var dashPos = transform.position + new Vector3(MoveVector.x, MoveVector.y) * _dashAmount;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveVector, _dashAmount, _dashRaycastLayerMask);
                if (hit.collider != null)
                {
                    dashPos = hit.point;
                }

                transform.position = dashPos;
                Instantiate(_dashAnim, transform.position, Quaternion.identity);
                _isDashing = false;
                _dashCooldownTimer = _dashCooldown;
            }
        }
    }

    public void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
        SceneManager.LoadScene("Afrodite Fight");
    }

    private void HandleShield()
    {
        _activateShieldTimer -= Time.deltaTime;
        if (_activateShieldTimer <= 0f && !_canActivateShield)
        {
            _canActivateShield = true;
        }
        if (_playerInputActions.MainShip.ActivateShield.IsPressed() && _canActivateShield)
        {
            _shield.SetActive(true);
            _isShieldEnabled = true;
        }
        if (!_playerInputActions.MainShip.ActivateShield.IsPressed())
        {
            _shield.SetActive(false);
            _isShieldEnabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CanTakeDamage)
        {
            if (other.CompareTag("Bullet"))
            {
                if (!_isShieldEnabled)
                {
                    TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
                }

                if (_isShieldEnabled)
                {
                    _canActivateShield = false;
                    _activateShieldTimer = _timeToActivateShield;
                    _shield.SetActive(false);
                    _isShieldEnabled = false;
                }

                other.GetComponent<BulletBase>().DestroyBullet();
            }

            if (other.CompareTag("AfroditeMainShip"))
            {
                TakeDamage(AfroditeController.Instance.CollisionDamage);
            }

            if (other.CompareTag("AfroditeMember"))
            {
                TakeDamage(AfroditeController.Instance.CollisionDamage);
            }
        }

        if (other.CompareTag("SatelliteLaserCollider") || other.CompareTag("Satellite"))
            Death();
    }
}