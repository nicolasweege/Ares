using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    public class Enemy1 : Enemy
    {
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _rigidbody2D.velocity = new Vector2(0f, -Speed);
        }

        private void Update()
        {
            Shoot();

            if (Health <= 0) Death();
        }

        protected void CreateShot()
        {
            var shot = Instantiate(_shotPf, _shotStartPos.position, Quaternion.identity);
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -shot.GetComponent<Shot>().GetSpeed());
        }

        private void Shoot()
        {
            bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
            if (!enemyIsVisible) return;

            _timeToShoot -= Time.deltaTime;

            if (_timeToShoot <= 0)
            {
                CreateShot();
                _timeToShoot = Random.Range(_minTimeToShoot, _maxTimeToShoot);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.tag)
            {
                case "InstanceDestroyer":
                    bool v = transform.position.y <= 0;
                    if (v) Destroy(gameObject);
                    break;

                case "Shot":
                    bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
                    if (enemyIsVisible)
                    {
                        LoseLife(other.GetComponent<Shot>().GetDefaultDamage());
                        other.GetComponent<Shot>().DestroyShot_2();
                    }
                    break;
            }
        }
    }
}