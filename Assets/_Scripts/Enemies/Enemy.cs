using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected GameObject explosionAnimation;
    public int defaultDamage = 1;
    protected new Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public int LoseLife(int damage)
    {
        return health -= damage;
    }

    public void Death()
    {
        Destroy(gameObject);
        Instantiate(explosionAnimation, transform.position, transform.rotation);
    }
}
