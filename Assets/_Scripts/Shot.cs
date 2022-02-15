using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected GameObject damageAnimation;
    public int defaultDamage;

    public void DestroyShot()
    {
        Destroy(gameObject);
        Instantiate(damageAnimation, transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "InstanceDestroyer":
                Destroy(gameObject);
                break;
        }
    }
}
