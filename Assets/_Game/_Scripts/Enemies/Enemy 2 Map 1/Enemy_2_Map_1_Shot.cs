using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Map_1_Shot : ShotBase
{
    [SerializeField] private Vector3 _rotation;

    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
        DeactiveShot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
            DestroyShot();
    }
}