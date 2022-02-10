using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] private GameObject deathAnimation;
    public int defaultDamage;

    public int LoseLife(int damage)
    {
        return health -= damage;
    }

    protected void CreateShot(GameObject shot, Transform shotStartPosition)
    {
        Instantiate(shot, shotStartPosition.position, shotStartPosition.rotation);
    }

    public void Death()
    {
        Destroy(gameObject);
        Instantiate(deathAnimation, transform.position, transform.rotation);
    }
}
