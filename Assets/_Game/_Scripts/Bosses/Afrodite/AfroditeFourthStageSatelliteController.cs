using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageSatelliteController : Singleton<AfroditeFourthStageSatelliteController>
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _laser1;
    [SerializeField] private GameObject _laser2;
    [SerializeField] private GameObject _laser3;
    [SerializeField] private GameObject _laser4;
    [SerializeField] private LayerMask _layerMask;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (_health <= 0)
        {
            DisableLasers();
        }
    }

    private void TakeDamage(int damage)
    {
        _health -= damage;
        foreach(SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
        {
            spr.enabled = false;
        }
    }

    private void DisableLasers()
    {
        _laser1.SetActive(false);
        _laser2.SetActive(false);
        _laser3.SetActive(false);
        _laser4.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<BulletBase>().DefaultDamage);
        }
    }
}