using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _shotPf;
    [SerializeField] private Transform _shotStartPos;
    private PlayerController _playerScript;

    private void Start()
    {
        _playerScript = GetComponent<PlayerController>();

        _playerScript.PlayerInputActions = new PlayerInputActions();
        _playerScript.PlayerInputActions.Player.Enable();
        _playerScript.PlayerInputActions.Player.Shoot.performed += PlayerShoot;
    }

    public void PlayerShoot(InputAction.CallbackContext context)
    {
        GameObject shot = Instantiate(_shotPf, _shotStartPos.position, _shotStartPos.rotation);
        shot.GetComponent<Rigidbody2D>().velocity = _playerScript.Aim.PlayerAim() * shot.GetComponent<Shot>().Speed;
    }
}