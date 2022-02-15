using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "InstanceDestroyer":
                Destroy(gameObject);
                break;

            case "Enemy":
                other.GetComponent<Enemy>().LoseLife(DefaultDamage);
                DestroyShot();
                break;

            case "Shot":
                Destroy(other.gameObject);
                DestroyShot();
                break;
        }
    }
}
