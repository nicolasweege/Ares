using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : ShotBase
{
    private Rigidbody2D _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DeactiveShot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shot"))
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
    }
}