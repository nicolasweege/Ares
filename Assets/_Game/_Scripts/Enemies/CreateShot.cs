using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    public class CreateShot : MonoBehaviour
    {
        private Enemy _enemyScript;

        private void Start()
        {
            _enemyScript = GetComponent<Enemy>();
        }

        public void EnemyCreateShot()
        {
            var shot = Instantiate(_enemyScript.ShotPf, _enemyScript.ShotStartPos.position, Quaternion.identity);
            Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();
            shotRb.velocity = new Vector2(0f, -shot.GetComponent<Shot>().GetSpeed());
        }
    }
}