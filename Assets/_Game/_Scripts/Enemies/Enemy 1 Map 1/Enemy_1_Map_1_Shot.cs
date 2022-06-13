using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Map_1_Shot : ShotBase
{
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DeactiveShot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
            DestroyShot();   
    }
}