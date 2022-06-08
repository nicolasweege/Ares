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
}