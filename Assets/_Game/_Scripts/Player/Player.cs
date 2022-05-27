using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _defaultDamage;
    private Rigidbody2D _rb;
    private Camera _cam;
    private bool _isGamepad;
    private PlayerInputActions _playerInputActions;

    public int Health { get => _health; set => _health = value; }
    public Rigidbody2D Rb { get => _rb; set => _rb = value; }
    public Camera Cam { get => _cam; set => _cam = value; }
    public PlayerInputActions PlayerInputActions { get => _playerInputActions; set => _playerInputActions = value; }

    #region Shared Components
    public Move Move { get => GetComponent<Move>(); }
    public Aim Aim { get => GetComponent<Aim>(); }
    public Death Death { get => GetComponent<Death>(); }
    public LoseLife LoseLife { get => GetComponent<LoseLife>(); }
    public Shoot Shoot { get => GetComponent<Shoot>(); }
    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = FindObjectOfType<Camera>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            LoseLife.PlayerLoseLife(other.gameObject.GetComponent<Enemy>().DefaultDamage);
            other.gameObject.GetComponent<Enemy>().Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Shot"))
        {
            LoseLife.PlayerLoseLife(other.GetComponent<Shot>().DefaultDamage);
            other.GetComponent<Shot>().DestroyShot();
        }

        if (other.gameObject.tag.Equals("Enemy"))
        {
            LoseLife.PlayerLoseLife(other.GetComponent<Enemy>().DefaultDamage);
            other.GetComponent<Enemy>().LoseLife(_defaultDamage);
        }
    }

    public void OnDeviceChange(PlayerInput playerInput) => _isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
}