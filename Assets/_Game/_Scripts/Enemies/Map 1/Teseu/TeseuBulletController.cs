using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeseuBulletController : BulletBase
{
    [SerializeField] private Vector3 _rotation;

    private void Update()
    {
        MoveShot();
        DeactiveShot();
        transform.Rotate(_rotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
            DestroyShot();
    }
}