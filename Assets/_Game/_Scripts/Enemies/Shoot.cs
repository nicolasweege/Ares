using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private GameObject _shotPf;
        private Enemy _enemyScript;

        private void Start()
        {
            _enemyScript = GetComponent<Enemy>();
        }

        public void EnemyShoot()
        {
            bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
            if (!isEnemyVisible)
                return;

            var timeToShoot = _enemyScript.TimeToShoot;
            timeToShoot -= Time.deltaTime;

            if (timeToShoot <= 0)
            {
                _enemyScript.CreateShot.EnemyCreateShot();
                timeToShoot = Random.Range(_enemyScript.MinTimeToShoot, _enemyScript.MaxTimeToShoot);
            }
        }
    }
}