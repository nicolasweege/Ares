using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private Vector3 _direction;

    private void Update()
    {
        transform.position += _direction * Time.deltaTime * _speed;
        transform.Rotate(_rotation * Time.deltaTime);
    }
}