using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageSatelliteController : Singleton<AfroditeFourthStageSatelliteController>
{
    [SerializeField] private int _health;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private FlashHitEffect _flashHitEffect;
    [NonSerialized] public bool IsFlashing = false;

    protected override void Awake()
    {
        base.Awake();

        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            spr.gameObject.AddComponent<AfroditeFourthStageSatelliteResetColor>();
    }

    private void Update()
    {
        IsFlashing = _flashHitEffect.IsFlashing;

        if (_health <= 0)
            Destroy(gameObject);
    }

    private void TakeDamage(int damage)
    {
        _health -= damage;
        _flashHitEffect.Flash();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
            other.GetComponent<BulletBase>().DestroyBullet();
        }
    }
}