using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float _time2Wait;
    private float _timer = 0f;
    private int _points;
    private int _level;

    private void Update()
    {
        CreateEnemies();
    }

    private void CreateEnemies()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            CreateEnemy(Random.Range(1, 5), 0);
            CreateEnemy(Random.Range(1, 2), 1);
            _timer = _time2Wait;
        }
    }

    private void CreateEnemy(int enemyAmaunt, int enemyIndex)
    {
        for (int i = 0; i < enemyAmaunt * 0.5f; i++)
        {
            Instantiate(_enemies[enemyIndex], new Vector3(Random.Range(-4f, 4f), Random.Range(6f, 15f), transform.position.z), transform.rotation);
        }
        for (int i = 0; i < enemyAmaunt * 0.1f; i++)
        {
            Instantiate(_enemies[enemyIndex], new Vector3(Random.Range(-8f, -4f), Random.Range(6f, 15f), transform.position.z), transform.rotation);
            Instantiate(_enemies[enemyIndex], new Vector3(Random.Range(4f, 8f), Random.Range(6f, 15f), transform.position.z), transform.rotation);
        }
    }
}
