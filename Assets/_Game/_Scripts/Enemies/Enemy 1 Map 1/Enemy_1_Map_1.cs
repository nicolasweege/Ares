using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Map_1 : EnemyBase
{
    [SerializeField] private GameObject _subEnemyPrefab;
    [SerializeField] private Transform _subEnemyStartPosRight;
    [SerializeField] private Transform _subEnemyStartPosLeft;
    [SerializeField] private float _timeToGenerateSubEnemies;
    private float _generateSubEnemiesTimer;
    private bool _isPlayerInRadar = false;
    private BoxCollider2D _boxCollider;

    protected override void Awake()
    {
        _generateSubEnemiesTimer = _timeToGenerateSubEnemies;
    }

    private void Update()
    {
        if (_isPlayerInRadar)
            GenerateSubEnemies();

        if (_health <= 0)
            Death();
    }

    private void GenerateSubEnemy(Transform subEnemyStartPos) => Instantiate(_subEnemyPrefab, subEnemyStartPos.position, Quaternion.identity);

    private void GenerateSubEnemies()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        _generateSubEnemiesTimer -= Time.deltaTime;
        if (_generateSubEnemiesTimer <= 0f)
        {
            GenerateSubEnemy(_subEnemyStartPosRight);
            GenerateSubEnemy(_subEnemyStartPosLeft);
            _generateSubEnemiesTimer = _timeToGenerateSubEnemies;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRadar = true;
            _boxCollider.size = new Vector2(20f, 20f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRadar = false;
            _boxCollider.size = new Vector2(15f, 15f);
            _generateSubEnemiesTimer = _timeToGenerateSubEnemies;
        }
    }
}