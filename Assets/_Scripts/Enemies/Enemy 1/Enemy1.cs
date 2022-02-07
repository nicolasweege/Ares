using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private float enemy1Speed = 2f;
    private Rigidbody2D enemy1Rb2D;

    private void Start()
    {
        enemy1Rb2D = GetComponent<Rigidbody2D>();

        enemy1Rb2D.velocity = new Vector2(0f, -enemy1Speed);
    }
}
