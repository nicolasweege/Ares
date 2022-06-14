using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Map_1_Shot : ShotBase
{
    private void Update()
    {
        MoveShot();
        DeactiveShot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
            DestroyShot();
    }
}