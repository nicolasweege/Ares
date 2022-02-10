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
        Instantiate(damageAnimation, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
