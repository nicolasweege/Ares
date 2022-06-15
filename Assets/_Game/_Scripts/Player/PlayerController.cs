using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _defaultDamage;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletStartingPos;
    [SerializeField] private GameObject _deathAnim;
    private bool _isGamepad;
    private Rigidbody2D _rigidbody;
    private Camera _camera;
    private PlayerInputActions _playerInputActions;

    public PlayerInputActions PlayerInputActions { get => _playerInputActions; set => _playerInputActions = value; }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = FindObjectOfType<Camera>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Shoot.performed += Shoot_performed;
    }

    private void Update()
    {
        Move();
        // Aim();

        if (_health <= 0)
            Death();
    }

    private int TakeDamage(int damage) => _health -= damage;

    private void Shoot_performed(InputAction.CallbackContext context)
    {
        var bulletInst = Instantiate(_bulletPrefab, _bulletStartingPos.position, _bulletStartingPos.rotation);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(Aim().x, Aim().y);
    }

    private Vector2 Aim()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - _rigidbody.position;
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

    public void OnDeviceChange(PlayerInput playerInput) => _isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
}