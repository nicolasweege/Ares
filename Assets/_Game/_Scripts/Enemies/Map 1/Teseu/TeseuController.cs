using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeseuController : EnemyBase
{
    [SerializeField] private string _state;
    [SerializeField] private GameObject _normalBulletPrefab;
    [SerializeField] private GameObject _especialBulletPrefab;
    [SerializeField] private Transform _normalBulletStartingPos;
    [SerializeField] private Transform _especialBulletStartingPosLeft;
    [SerializeField] private Transform _especialBulletStartingPosRight;
    [SerializeField] private float _timeToNormalShoot;
    [SerializeField] private float _timeToEspecial;
    [SerializeField] private float _timeToBreak;
    private float _breakTimer;
    private float _normalShootTimer;
    private float _especialShootTimer;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _state = "idle";
        _especialShootTimer = _timeToEspecial;
        _breakTimer = _timeToBreak;
    }

    private void Update()
    {
        HandleState();

        if (_health <= 0)
            Death();
    }

    private void HandleState()
    {
        switch (_state)
        {
            case "idle":
                break;

            case "break_to_especial":
                SetBreakState("especial_shoot", 0.5f);
                break;

            case "break_to_normal":
                SetBreakState("following_player", _timeToBreak);
                break;

            case "following_player":
                FollowPlayer();
                HandleNormalShoot();

                _especialShootTimer -= Time.deltaTime;
                if (_especialShootTimer <= 0f)
                {
                    _state = "break_to_especial";
                    _especialShootTimer = _timeToEspecial;
                }
                break;

            case "especial_shoot":
                HandleEspecialShoot();
                break;
        }
    }

    private void SetBreakState(string nextState, float time)
    {
        _breakTimer -= Time.deltaTime;
        if (_breakTimer <= 0f)
        {
            _state = nextState;
            _breakTimer = time;
        }
    }

    private void HandleNormalShoot()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        _normalShootTimer -= Time.deltaTime;
        if (_normalShootTimer <= 0f)
        {
            GenerateBullet(_normalBulletStartingPos, _normalBulletPrefab);
            _normalShootTimer = _timeToNormalShoot;
        }
    }

    private void HandleEspecialShoot()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        GenerateBullet(_especialBulletStartingPosLeft, _especialBulletPrefab);
        GenerateBullet(_especialBulletStartingPosRight, _especialBulletPrefab);
        _state = "break_to_normal";
    }

    private void GenerateBullet(Transform bulletStartingPos, GameObject bulletPrefab)
    {
        if (PlayerAttackShipController.Instance == null)
            return;

        GameObject bulletInst = Instantiate(bulletPrefab, bulletStartingPos.position, Quaternion.identity);
        Vector2 bulletDir = PlayerAttackShipController.Instance.transform.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle + 90f);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _state = "following_player";
            _boxCollider.size = new Vector2(17f, 17f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _state = "idle";
            _especialShootTimer = _timeToEspecial;
            _boxCollider.size = new Vector2(10f, 10f);
        }
    }
}