using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Map_1_Shot : ShotBase
{
    [SerializeField] private Vector3 _rotation;
    private Rigidbody2D _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
        DeactiveShot();
    }
}