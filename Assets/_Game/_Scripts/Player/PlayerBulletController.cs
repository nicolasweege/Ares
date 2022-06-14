using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : BulletBase
{
    private void Update()
    {
        MoveShot();
        DeactiveShot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            DestroyShot();
        }

        if (other.CompareTag("Enemy"))
        {
            bool isEnemyVisible = other.GetComponentInChildren<SpriteRenderer>().isVisible;
            if (!isEnemyVisible)
                return;

            other.GetComponent<EnemyBase>().TakeDamage(DefaultDamage);
            DestroyShot();
        }

        if (other.CompareTag("Asteroid"))
            DestroyShot();
    }
}