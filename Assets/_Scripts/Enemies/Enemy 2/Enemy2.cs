using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.velocity = new Vector2(0f, -speed);
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Death();
        }
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
