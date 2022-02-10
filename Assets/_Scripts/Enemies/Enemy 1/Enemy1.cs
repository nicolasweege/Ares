using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    [SerializeField] private GameObject shot;
    [SerializeField] private Transform shotStartPosition;
    [SerializeField] private int timeToCount;
    [SerializeField] private int timeToShoot;
    private new Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.velocity = new Vector2(0f, -speed);
    }

    private void FixedUpdate()
    {
        var enemy1IsVisible = GetComponentInChildren<SpriteRenderer>().isVisible;

        if (enemy1IsVisible)
        {
            timeToCount++;

            if (timeToCount >= timeToShoot)
            {
                Shoot(shot, shotStartPosition);
                timeToCount = 0;
            }
        }

        if (health <= 0) Death();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "EnemyDestroyer":
                Destroy(gameObject);
                break;
        }
    }
}
