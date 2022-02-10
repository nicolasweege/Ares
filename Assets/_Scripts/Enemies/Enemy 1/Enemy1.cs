using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    [SerializeField] private GameObject shot;
    [SerializeField] private Transform shotStartPosition;
    [SerializeField] private int timeToCount = 0;
    [SerializeField] private int timeToShoot = 120;

    private void Start()
    {
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
                Instantiate(shot, shotStartPosition.position, shotStartPosition.rotation);
                timeToCount = 0;
            }
        }

        if (health <= 0) Death();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "ShotCollider":
                Destroy(gameObject);
                break;
        }
    }
}
