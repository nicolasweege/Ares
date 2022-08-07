using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Linq;
using Cyan;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerMainShipController : Singleton<PlayerMainShipController>
{
    #region Variables
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
    [SerializeField] private List<Image> _heartImages;

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
    private float _fullScreenIntensity = 0f;
    private bool _dmgAnimEnabled = false;
    [NonSerialized] public PlayerInputActions PlayerInputActions;
    [NonSerialized] public bool CanTakeDamage = true;
    [NonSerialized] public bool CanResetColors = true;
    [NonSerialized] public Vector2 MoveVector;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.MainShip.Enable();
        DisableShield();
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
        // HandleShield();
        HandleDamange();
        HandleDamageAnimation();
        HandleHealthHUD();

        if (PlayerInputActions.MainShip.Dash.IsPressed())
        {
            if (_dashCooldownTimer <= 0f)
            {
                Instantiate(_dashAnim, transform.position, Quaternion.identity);
                _isDashing = true;
            }
        }

        _shootTimer -= Time.deltaTime;
        if (PlayerInputActions.MainShip.NormalShoot.IsPressed() && _canMove)
        {
            if (_shootTimer <= 0f)
            {
                GenerateBullet();
                SoundManager.PlaySound(SoundManager.Sound.PlayerShoot, transform.position);
                _shootTimer = _timeToShoot;
            }
        }

        if (_health <= 0)
            HandleDeath();
    }

    private void HandleHealthHUD()
    {
        for (int i = 0; i < _heartImages.Count; i++)
        {
            if (i < _health)_heartImages[i].enabled = true;
            else _heartImages[i].enabled = false;
        }
    }

    private void HandleDamageAnimation()
    {
        if (_dmgAnimEnabled)
        {
            if (_fullScreenIntensity < 0.8f)
                _fullScreenIntensity += 0.08f;
        }
        else
        {
            if (_fullScreenIntensity > 0f)
                _fullScreenIntensity -= 0.017f;
            /*if (_fullScreenIntensity <= 0f)
                _renderer2DData.rendererFeatures[0].SetActive(false);*/
        }
        foreach (var renderObjSetting in _renderer2DData.rendererFeatures.OfType<Blit>())
            renderObjSetting.settings.blitMaterial.SetFloat("_FullScreenIntensity", _fullScreenIntensity);
    }

    public void TakeDamage()
    {
        if (CanTakeDamage)
        {
            _health -= 1;
            CinemachineManager.Instance.ScreenShakeEvent(_screenShakeEvent);
            CanTakeDamage = false;
            _canMove = false;
            _isDashing = false;
            _dashCooldownTimer = _dashCooldown;
            _dmgAnimEnabled = true;
            _renderer2DData.rendererFeatures[0].SetActive(true);
            _isFlickerEnabled = true;
            ColorFlicker(150);
            AssetsManager.Instance.PlayerIsTakingDamageSnapshot.TransitionTo(0.01f);
            SoundManager.PlaySound(SoundManager.Sound.PlayerTakingDamage);
        }
    }

    private async void ColorFlicker(int millisecondsDelay)
    {
        while (_isFlickerEnabled == true)
        {
            CanResetColors = false;
            foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                spr.color = _flashColor;
            _turbineFlame.Stop();
            await Task.Delay(millisecondsDelay);
            CanResetColors = true;
            _turbineFlame.Play();
            await Task.Delay(millisecondsDelay);
            ColorFlicker(150);
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
        else ColorFlicker(150);

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
        else
        {
            AssetsManager.Instance.PlayerIsNotTakingDamageSnapshot.TransitionTo(2f);
            _dmgAnimEnabled = false;
        }
    }

    private void HandleTurbineFlame()
    {
        if (PlayerInputActions.MainShip.Movement.IsPressed() && CanTakeDamage)
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
        Vector2 mousePos = Utils.GetMouseWorldPosition();
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
            MoveVector = PlayerInputActions.MainShip.Movement.ReadValue<Vector2>().normalized;
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

    public void HandleDeath()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
        SceneManager.LoadScene("Afrodite Fight");
    }

    private void HandleShield()
    {
        _activateShieldTimer -= Time.deltaTime;
        if (_activateShieldTimer <= 0f && !_canActivateShield)
            _canActivateShield = true;
        if (PlayerInputActions.MainShip.ActivateShield.IsPressed() && _canActivateShield)
            EnableShield();
        if (!PlayerInputActions.MainShip.ActivateShield.IsPressed())
            DisableShield();
    }

    private void EnableShield()
    {
        _shield.SetActive(true);
        _isShieldEnabled = true;
    }

    private void DisableShield()
    {
        _shield.SetActive(false);
        _isShieldEnabled = false;
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if (CanTakeDamage)
        {
            if (other.CompareTag("Bullet"))
            {
                if (!_isShieldEnabled)
                {
                    TakeDamage();
                }

                if (_isShieldEnabled)
                {
                    _canActivateShield = false;
                    _activateShieldTimer = _timeToActivateShield;
                    DisableShield();
                }

                other.GetComponent<BulletBase>().DestroyBullet();
            }

            if (other.CompareTag("AfroditeMainShip"))
            {
                TakeDamage();
            }

            if (other.CompareTag("AfroditeMember"))
            {
                TakeDamage();
            }
        }

        if (other.CompareTag("SatelliteLaserCollider") || other.CompareTag("Satellite"))
            HandleDeath();
    }
}