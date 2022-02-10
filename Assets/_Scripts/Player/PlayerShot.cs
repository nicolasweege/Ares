using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] private float playerShotSpeed = 10f;
    [SerializeField] private GameObject damageAnimation;
    [SerializeField] private int defaultDamage = 1;
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
            case "PlayerShotCollider":
                Destroy(gameObject);
                break;

            case "Enemy":
                Instantiate(damageAnimation, transform.position, transform.rotation);
                other.GetComponent<Enemy>().LoseLife(defaultDamage);
                Destroy(gameObject);
                break;

            case "Enemy1Shot":
                Instantiate(damageAnimation, transform.position, transform.rotation);
                Destroy(other.gameObject);
                Destroy(gameObject);
                break;
        }
    }
}
