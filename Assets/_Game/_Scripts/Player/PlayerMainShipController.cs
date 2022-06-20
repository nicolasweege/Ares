using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMainShipController : Singleton<PlayerMainShipController>
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _attackShipPrefab;
    [SerializeField] private GameObject _deathAnim;
    [SerializeField] private GameObject _cinemachineCamera;
    [SerializeField] private GameObject _portalPrefab;
    private CinemachineVirtualCamera _virtualCamera;
    private PlayerInputActions _playerInputActions;
    private bool _isPlayerInSubShip = false;

    protected override void Awake()
    {
        base.Awake();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.MainShip.Enable();
    }

    private void Start()
    {
        _virtualCamera = _cinemachineCamera.GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (!_isPlayerInSubShip)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.R))
                Instantiate(_portalPrefab, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f), Quaternion.identity);
        }

        if (_playerInputActions.MainShip.ChangeToSubShip.IsPressed() && !_isPlayerInSubShip)
        {
            ChangeToAttackShip();
            _isPlayerInSubShip = true;
        }
    }

    private int TakeDamage(int damage) => _health -= damage;

    private void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
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

    private void ChangeToAttackShip()
    {
        var shipInst = Instantiate(_attackShipPrefab, new Vector3(transform.position.x + 5f, transform.position.y -5f), Quaternion.identity);
        _virtualCamera.m_Lens.OrthographicSize = 7f;
        _virtualCamera.Follow = shipInst.transform;
        _playerInputActions.MainShip.Disable();
    }
}