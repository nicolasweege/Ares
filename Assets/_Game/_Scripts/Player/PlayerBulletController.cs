using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : BulletBase
{
    private void Update()
    {
        MoveBullet();
        DeactiveBullet();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            DestroyBullet();
        }

        if (other.CompareTag("Enemy"))
        {
            bool isEnemyVisible = other.GetComponentInChildren<SpriteRenderer>().isVisible;
            if (!isEnemyVisible)
                return;

            other.GetComponent<EnemyBase>().TakeDamage(DefaultDamage);
            DestroyBullet();
        }

        if (other.CompareTag("Asteroid"))
            DestroyBullet();

        if (other.CompareTag("Satellite"))
            DestroyBullet();

        if (other.CompareTag("ArenaCollider"))
            DestroyBullet();
    }
}