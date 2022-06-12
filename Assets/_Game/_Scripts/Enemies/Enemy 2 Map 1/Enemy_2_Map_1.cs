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
    private float _normalShootTimer;
    private float _especialShootTimer;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void Start()
    {
        _state = "idle";
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

            case "following_player":
                FollowPlayer();
                HandleNormalShoot();

                _especialShootTimer -= Time.deltaTime;
                if (_especialShootTimer <= 0f)
                {
                    _state = "especial_shoot";
                    _especialShootTimer = _timeToEspecial;
                }
                break;

            case "especial_shoot":
                HandleEspecialAttack();
                break;
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
        _state = "following_player";
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
            _boxCollider.size = new Vector2(15f, 15f);
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