using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NPlayer
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private GameObject _shot;
        [SerializeField] private Transform _shotStartPosition;
        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();

            _player.PlayerInputActions = new PlayerInputActions();
            _player.PlayerInputActions.Player.Enable();
            _player.PlayerInputActions.Player.Shoot.performed += ShootPlayer;
        }

        public void ShootPlayer(InputAction.CallbackContext context)
        {
            GameObject shot = Instantiate(_shot, _shotStartPosition.position, _shotStartPosition.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = _player.Aim.AimPlayer() * shot.GetComponent<Shot>().GetSpeed();
        }
    }
}