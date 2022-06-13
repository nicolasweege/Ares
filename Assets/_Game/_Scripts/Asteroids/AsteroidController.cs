using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _rotation;

    private void Update()
    {
        transform.position += new Vector3(1f, -1f, transform.position.z) * Time.deltaTime * _speed;
        transform.Rotate(_rotation * Time.deltaTime);
    }
}