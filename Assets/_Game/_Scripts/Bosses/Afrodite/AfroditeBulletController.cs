using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeBulletController : BulletBase
{
    protected override void Awake()
    {
        base.Awake();
        _destroyVisibleBulletTimer = _timeToDestroyVisibleBullet;
    }

    private void Update()
    {
        MoveBullet();
        DeactiveBullet();
        DestroyVisibleBullet();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
            DestroyBullet();
    }
}