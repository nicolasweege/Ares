using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float _time2Wait;
    [SerializeField] private int _coins;
    [SerializeField] private int _enemiesAmount = 0;
    private float _timer = 0f;

    private void Update()
    {
        CreateEnemies(1, 3);

        //Debug.Log("Coins: " + _coins);
    }

    private void CreateEnemies(int minEnemiesAmount, int maxEnemiesAmount)
    {
        int enemiesAmount = Random.Range(minEnemiesAmount, maxEnemiesAmount);

        _timer -= Time.deltaTime;

        if (_timer > 0) return;

        if (_enemiesAmount < enemiesAmount)
        {
            CreateEnemy(enemiesAmount, 0);
            CreateEnemy(1, 1);
            _timer = _time2Wait;
        }
    }

    private void CreateEnemy(int enemiesAmount, int enemyIndex)
    {
        for (int i = 0; i < enemiesAmount * 0.5f; i++)
        {
            Instantiate(_enemies[enemyIndex], new Vector3(Random.Range(-4f, 4f), Random.Range(6f, 15f), transform.position.z), transform.rotation);
        }
        for (int i = 0; i < enemiesAmount * 0.1f; i++)
        {
            Instantiate(_enemies[enemyIndex], new Vector3(Random.Range(-8f, -4f), Random.Range(6f, 15f), transform.position.z), transform.rotation);
            Instantiate(_enemies[enemyIndex], new Vector3(Random.Range(4f, 8f), Random.Range(6f, 15f), transform.position.z), transform.rotation);
        }
    }

    public int GetCoins(int coinsValue)
    {
        return _coins += coinsValue;
    }
}
