using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private float enemy1Speed = 2f;
    [SerializeField] private GameObject enemy1ShotPrefab;
    [SerializeField] private Transform shotStartPosition;
    [SerializeField] private int timeToCount = 0;
    [SerializeField] private int timeToShoot = 120;
    [SerializeField] private int enemy1Health = 2;
    [SerializeField] private GameObject enemy1ExplosionAnimationPrefab;
    private Rigidbody2D enemy1Rb2D;

    private void Start()
    {
        enemy1Rb2D = GetComponent<Rigidbody2D>();

        enemy1Rb2D.velocity = new Vector2(0f, -enemy1Speed);
    }

    private void FixedUpdate()
    {
        timeToCount++;

        if (timeToCount >= timeToShoot)
        {
            Instantiate(enemy1ShotPrefab, shotStartPosition.position, shotStartPosition.rotation);
            timeToCount = 0;
        }

        if (enemy1Health <= 0)
        {
            Instantiate(enemy1ExplosionAnimationPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public int LoseLife(int damage)
    {
        return enemy1Health -= damage;
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
