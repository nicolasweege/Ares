using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "InstanceDestroyer":
                Destroy(gameObject);
                break;

            case "Shot":
                Destroy(other.gameObject);
                DestroyShot();
                break;
        }
    }
}
