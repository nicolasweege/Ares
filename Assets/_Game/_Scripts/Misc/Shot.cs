using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] protected GameObject _damageAnimation;
    [SerializeField] private float _speed;
    [SerializeField] private int _defaultDamage;

    public float GetSpeed()
    {
        return _speed;
    }

    public int GetDefaultDamage()
    {
        return _defaultDamage;
    }

    public void DestroyShot()
    {
        Destroy(gameObject);
        Instantiate(_damageAnimation, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "InstanceDestroyer":
                Destroy(gameObject);
                break;
        }
    }
}