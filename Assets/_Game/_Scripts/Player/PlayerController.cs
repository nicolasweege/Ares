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
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    #region Variables
    [Header("General")]
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
    [SerializeField] private GameObject _laserBeam;
    [SerializeField] private ParticleSystem _turbineFlame;
    [SerializeField] private float _dashAmount;
    [SerializeField] private LayerMask _dashRaycastLayerMask;
    [SerializeField] private Transform _aimTransform;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _timeToFinishDash;
    [SerializeField] private float _timeToCanTakeDamage;
    [SerializeField] private float _timeToCanMove;
    [SerializeField] private Color _flashColor;
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private Renderer2DData _renderer2DData;
    [SerializeField] private List<Image> _heartImages;

    [Header("Events")]
    [SerializeField] private UnityEvent _screenShakeEvent;

    private float _shootTimer;
    private float _canMoveTimer;
    private float _canTakeDamageTimer;
    private float _dashCooldownTimer;
    private bool _isDashing = false;
    private bool _canMove = true;
    private bool _isFlickerEnabled = false;
    private float _fullScreenIntensity = 0f;
    private bool _dmgAnimEnabled = false;
    private bool _canDie = true;
    private Vector3 _dashPos;
    [NonSerialized] public PlayerInputActions PlayerInputActions;
    [NonSerialized] public bool CanTakeDamage = true;
    [NonSerialized] public bool CanResetColors = true;
    [NonSerialized] public Vector2 MoveVector;

    [Header("Gamepad Settings")]
    [SerializeField] private float _controllerDeadzone = 0.1f;
    [SerializeField] private Transform _aimObjectTransform;
    [NonSerialized] public Vector2 _playerDirGamepadMode;
    public bool IsGamepad;
    private Vector2 _aimVector;
    private PlayerInput _playerInputComponent;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        _playerInputComponent = GetComponent<PlayerInput>();
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.MainShip.Enable();
        PlayerInputActions.MainShip.Dash.performed += Dash;
        _canTakeDamageTimer = _timeToCanTakeDamage;
        _canMoveTimer = _timeToCanMove;

        Cursor.SetCursor(_cursorTexture, new Vector2(_cursorTexture.width / 2, _cursorTexture.height / 2), CursorMode.ForceSoftware);

        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.gameObject.AddComponent<PlayerResetColor>();
    }

    private void Dash(InputAction.CallbackContext context) {
        if (!_isDashing && _dashCooldownTimer <= 0f && MoveVector != Vector2.zero) {
            _isDashing = true;

            foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                spr.enabled = false;

            _dashPos = transform.position + new Vector3(MoveVector.x, MoveVector.y) * _dashAmount;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveVector, _dashAmount, _dashRaycastLayerMask);
            if (hit.collider != null && !hit.collider.gameObject.CompareTag("AfroditeMember"))
                _dashPos = hit.point;
            
            Instantiate(_dashAnim, transform.position, Quaternion.identity);

            Invoke(nameof(FinishDash), _timeToFinishDash);
        }
    }

    private void FinishDash() {
        transform.position = _dashPos;
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.enabled = true;
        Instantiate(_dashAnim, transform.position, Quaternion.identity);
        _isDashing = false;
        _dashCooldownTimer = _dashCooldown;
    }

    private void Update()
    {
        HandleMove();
        HandleAim();
        HandleTurbineFlame();
        HandleDamange();
        HandleDamageAnimation();
        HandleHealthHUD();

        if (Input.GetKeyDown(KeyCode.G)) {
            HandleDeath();
        }

        _shootTimer -= Time.deltaTime;
        if (PlayerInputActions.MainShip.NormalShoot.IsPressed() && _canMove) {
            if (_shootTimer <= 0f) {
                GenerateBullet();
                SoundManager.PlaySound(SoundManager.Sound.PlayerShoot, transform.position);
                _shootTimer = _timeToShoot;
            }
        }

        if (_health <= 0 && _canDie) {
            FunctionTimer.Create(HandleDeath, 0.1f);
            _canDie = false;
        }
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
            if (_fullScreenIntensity < 0.5f)
                _fullScreenIntensity += 0.08f;
        }
        else
        {
            if (_fullScreenIntensity > 0f)
                _fullScreenIntensity -= 0.017f;
            if (_fullScreenIntensity <= 0f)
                _renderer2DData.rendererFeatures[0].SetActive(false);
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
        if (_isDashing) {
            _turbineFlame.Stop();
            return;
        }

        if (PlayerInputActions.MainShip.Movement.IsPressed() && CanTakeDamage) {
            _turbineFlame.Play();
        } else _turbineFlame.Stop();
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
        _aimVector = PlayerInputActions.MainShip.Aim.ReadValue<Vector2>();

        if (IsGamepad) {
            Utils.SetMouseInvisible();
            Vector2 playerDir;

            if (Math.Abs(_aimVector.x) > _controllerDeadzone || Math.Abs(_aimVector.y) > _controllerDeadzone) {
                playerDir = Vector2.right * _aimVector.x + Vector2.up * _aimVector.y;
                playerDir.Normalize();
                _playerDirGamepadMode = playerDir;
                float playerAngle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg - 90f;
                if (playerDir.sqrMagnitude > 0.0f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, playerAngle), _turnSpeed * Time.deltaTime);
                return playerDir;
            } else {
                playerDir = Vector2.zero;
                playerDir.Normalize();
                _playerDirGamepadMode = playerDir;
                return playerDir;
            }
        }

        Utils.SetMouseVisible();
        Vector2 mousePos = Utils.GetMouseWorldPosition();
        Vector2 lookDir = mousePos - new Vector2(transform.position.x, transform.position.y);
        lookDir.Normalize();
        _playerDirGamepadMode = lookDir;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, lookAngle), _turnSpeed * Time.deltaTime);
        return lookDir;
    }

    private void HandleMove()
    {
        if (_canMove) {
            MoveVector = PlayerInputActions.MainShip.Movement.ReadValue<Vector2>().normalized;
            transform.position += new Vector3(MoveVector.x, MoveVector.y) * Time.deltaTime * _speed;
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void HandleDeath()
    {
        FunctionTimer.Create(() => CinemachineManager.Instance.ZoomIn(5f, 5f), 0.5f);
        FunctionTimer.Create(() => CinematicBars.Instance.Show(540f, 0.2f), 0.5f);
        FunctionTimer.Create(() => CinematicBars.Instance.Hide(0.2f), 3f);
        FunctionTimer.Create(() => CinemachineManager.Instance.ZoomOut(7f, 5f), 3f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (CanTakeDamage)
        {
            if (other.CompareTag("Bullet") && !_isDashing) {
                TakeDamage();
                other.GetComponent<BulletBase>().DestroyBullet();
            }

            if (other.CompareTag("AfroditeMainShip"))
                TakeDamage();

            if (other.CompareTag("AfroditeMember") && !_isDashing)
                TakeDamage();
        }

        if (other.CompareTag("SatelliteLaserCollider") || other.CompareTag("Satellite")) {
            SceneManager.LoadScene("Afrodite Fight");
        }
    }

    private void OnDestroy() {
        _renderer2DData.rendererFeatures[0].SetActive(false);
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
        PlayerInputActions.MainShip.Dash.performed -= Dash;
        PlayerInputActions.Disable();
        PlayerInputActions = null;
    }

    public void OnDeviceChange(PlayerInput playerInput) {
        IsGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}