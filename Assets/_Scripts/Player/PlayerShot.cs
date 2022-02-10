using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.velocity = new Vector2(0, speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "PlayerDestroyer":
                Destroy(gameObject);
                break;

            case "Enemy":
                other.GetComponent<Enemy>().LoseLife(defaultDamage);
                DestroyShot(damageAnimation);
                break;

            case "Shot":
                Destroy(other.gameObject);
                DestroyShot(damageAnimation);
                break;
        }
    }
}
