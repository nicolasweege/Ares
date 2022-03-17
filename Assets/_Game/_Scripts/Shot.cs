using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] protected GameObject _damageAnimation;
    public float Speed;
    public int DefaultDamage;

    public void DestroyShot()
    {
        Destroy(gameObject);
        Instantiate(_damageAnimation, transform.position, transform.rotation);
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