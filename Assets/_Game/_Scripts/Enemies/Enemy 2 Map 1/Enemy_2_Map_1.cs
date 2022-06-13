using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Map_1 : EnemyBase
{
    [SerializeField] private string _state;
    [SerializeField] private GameObject _normalShotPrefab;
    [SerializeField] private GameObject _especialShotPrefab;
    [SerializeField] private Transform _normalShotStartPos;
    [SerializeField] private Transform _especialShotStartPosLeft;
    [SerializeField] private Transform _especialShotStartPosRight;
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
                HandleEspecialAttack();
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
            GenerateShot(_normalShotStartPos, _normalShotPrefab);
            _normalShootTimer = _timeToNormalShoot;
        }
    }

    private void HandleEspecialAttack()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        GenerateShot(_especialShotStartPosLeft, _especialShotPrefab);
        GenerateShot(_especialShotStartPosRight, _especialShotPrefab);
        _state = "break_to_normal";
    }

    private void GenerateShot(Transform shotStartPos, GameObject shotPrefab)
    {
        if (PlayerController.Instance == null)
            return;

        GameObject shotInst = Instantiate(shotPrefab, shotStartPos.position, Quaternion.identity);
        Vector2 shotDir = PlayerController.Instance.transform.position - shotInst.transform.position;
        shotDir.Normalize();
        float shotAngle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        shotInst.transform.rotation = Quaternion.Euler(0f, 0f, shotAngle + 90f);
        shotInst.GetComponent<Rigidbody2D>().velocity = shotDir * shotInst.GetComponent<ShotBase>().Speed;
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