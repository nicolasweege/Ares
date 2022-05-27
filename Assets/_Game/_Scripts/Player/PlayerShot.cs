using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
    private Rigidbody2D _rb;

    private void Start() => _rb = GetComponent<Rigidbody2D>();

    private void Update() => DeactiveShot();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Shot"))
        {
            Destroy(other.gameObject);
            DestroyShot();
        }
    }
}