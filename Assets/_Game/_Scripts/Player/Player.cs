using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _shot;
    [SerializeField] private Transform _shotStartPosition;
    [SerializeField] private GameObject _deathAnimation;
    private PlayerInputActions _playerInputActions;

    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Shoot.performed += Shoot;
    }

    private void Update()
    {
        Moving();
        Etc();

        if (_health <= 0) Death();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        GameObject shot = Instantiate(_shot, _shotStartPosition.position, _shotStartPosition.rotation);
        shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, shot.GetComponent<Shot>().Speed);
    }

    private void Moving()
    {
        Vector2 inputVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        inputVector.Normalize();

        transform.position += new Vector3(inputVector.x, inputVector.y, transform.position.z) * Time.deltaTime * _speed;
    }

    private void Etc()
    {
        if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, transform.position.z);
        }
    }

    private int LoseLife(int damage)
    {
        return _health -= damage;
    }

    private void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnimation, transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                LoseLife(other.GetComponent<Enemy>().DefaultDamage);
                other.GetComponent<Enemy>().Death();
                break;

            case "Shot":
                LoseLife(other.GetComponent<Shot>().DefaultDamage);
                other.GetComponent<Shot>().DestroyShot();
                break;
        }
    }
}
