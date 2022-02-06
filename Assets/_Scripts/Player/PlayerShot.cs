using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] private float playerShotSpeed = 5f;
    private Rigidbody2D playerShotRb2D;

    private void Start()
    {
        playerShotRb2D = GetComponent<Rigidbody2D>();

        playerShotRb2D.velocity = new Vector2(0, playerShotSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "ShotCollider":
                Destroy(gameObject);
                break;

            case "Enemy1Shot":
                Destroy(other.gameObject);
                Destroy(gameObject);
                break;
        }
    }
}
