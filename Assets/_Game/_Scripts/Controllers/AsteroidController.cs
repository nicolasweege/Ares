using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _direction;

    private void Update()
    {
        transform.position += _direction * Time.deltaTime * _speed;
    }
}