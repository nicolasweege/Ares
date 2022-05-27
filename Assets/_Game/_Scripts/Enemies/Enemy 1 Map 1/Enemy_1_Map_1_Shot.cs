using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Map_1_Shot : Shot
{
    private Rigidbody2D _rb;

    private void Start() => _rb = GetComponent<Rigidbody2D>();

    private void Update() => DeactiveShot();
}