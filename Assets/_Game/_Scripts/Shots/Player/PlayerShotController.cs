using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : PlayerShotBase
{
    private void Awake() => _rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        DeactiveShot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Shot"))
        {
            Destroy(other.gameObject);
            DestroyShot();
        }

        if (other.tag.Equals("Enemy"))
        {
            bool isEnemyVisible = other.GetComponentInChildren<SpriteRenderer>().isVisible;
            if (!isEnemyVisible)
                return;

            other.GetComponent<Enemy>().LoseLife(Stats.DefaultDamage);
            DestroyShot();
        }
    }
}