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
            DestroyBullet();

        if (other.CompareTag("AfroditeMainShip"))
        {
            if (AfroditeController.Instance.CurrentState != AfroditeController.Instance.DeathState)
            {
                AfroditeController.Instance.TakeDamage(_defaultDamage);
                DestroyBullet();
            }
        }

        if (other.CompareTag("Satellite"))
            DestroyBullet();

        if (other.CompareTag("SatelliteLaserCollider"))
            DestroyBullet();

        if (other.CompareTag("ArenaCollider"))
            DestroyBullet();
    }
}