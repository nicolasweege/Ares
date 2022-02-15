using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected float _speed;
    [SerializeField] protected float timeToShoot;
    [SerializeField] protected float minTimeToShoot;
    [SerializeField] protected float maxTimeToShoot;
    [SerializeField] protected GameObject shot;
    [SerializeField] protected Transform shotStartPosition;
    [SerializeField] private GameObject deathAnimation;
    public int defaultDamage;

    public int LoseLife(int damage)
    {
        return _health -= damage;
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

    protected void Shoot()
    {
        bool enemyIsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;

        if (enemyIsVisible)
        {
            timeToShoot -= Time.deltaTime;

            if (timeToShoot <= 0)
            {
                CreateShot(shot, shotStartPosition);
                timeToShoot = Random.Range(minTimeToShoot, maxTimeToShoot);
            }
        }
    }
}
