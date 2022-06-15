using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeseuBulletController : BulletBase
{
    [SerializeField] private Vector3 _rotation;

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
        transform.Rotate(_rotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
            DestroyBullet();
    }
}