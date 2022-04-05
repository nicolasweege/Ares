using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
    private float _timeToDestroyShot = 1f;

    private void Update()
    {
        _timeToDestroyShot -= Time.deltaTime;
        if (_timeToDestroyShot <= 0f && !GetComponentInChildren<SpriteRenderer>().isVisible) 
        {
            Destroy(gameObject);
            string objectName = "Player's shot";
            Debug.Log($"{objectName} destroyed");
        }
    }

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