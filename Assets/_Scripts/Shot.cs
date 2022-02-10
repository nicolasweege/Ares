using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected GameObject damageAnimation;
    public int defaultDamage;
    protected new Rigidbody2D rigidbody2D;

    protected void DestroyShot(GameObject damageAnimation)
    {
        Instantiate(damageAnimation, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
