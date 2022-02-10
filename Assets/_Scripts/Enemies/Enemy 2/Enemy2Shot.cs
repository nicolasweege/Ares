using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Shot : Shot
{
    private new Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.velocity = new Vector2(0f, -speed);
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
