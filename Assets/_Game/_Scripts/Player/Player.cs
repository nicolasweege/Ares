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
    [SerializeField] private Camera _camera;
    private Rigidbody2D _rb2D;
    private bool _isGamepad;
    public PlayerInputActions PlayerInputActions;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();

        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Player.Enable();
        PlayerInputActions.Player.Shoot.performed += Shoot;
    }

    private void Update()
    {
        Move();
        Aim();
        
        if (_health <= 0) Death();
    }

    private void Move()
    {
        Vector2 movementInputVector = PlayerInputActions.Player.Movement.ReadValue<Vector2>();
        movementInputVector.Normalize();

        transform.position += new Vector3(movementInputVector.x, movementInputVector.y, transform.position.z) * Time.deltaTime * _speed;
    }

    private Vector2 Aim()
    {
        Vector2 mousePosition;
        Vector2 lookDirection;
        float lookAngle;

        mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = mousePosition - _rb2D.position;
        lookDirection.Normalize();

        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        return lookDirection;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        GameObject shot = Instantiate(_shot, _shotStartPosition.position, _shotStartPosition.rotation);
        shot.GetComponent<Rigidbody2D>().velocity = Aim() * shot.GetComponent<Shot>().Speed;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                LoseLife(other.gameObject.GetComponent<Enemy>().DefaultDamage);
                other.gameObject.GetComponent<Enemy>().Death();
                FindObjectOfType<EnemiesGenerator>().GetCoins(other.gameObject.GetComponent<Enemy>().CoinsValue);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Shot":
                LoseLife(other.GetComponent<Shot>().DefaultDamage);
                other.GetComponent<Shot>().DestroyShot();
                break;
        }
    }

    public void OnDeviceChange(PlayerInput playerInput)
    {
        _isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}