using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NPlayer
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D _rb2D;
        private Camera _camera;
        private bool _isGamepad;

        public int Health;
        public PlayerInputActions PlayerInputActions;

        #region Shared Components
        public Move Move { get => GetComponent<Move>(); }
        public Aim Aim { get => GetComponent<Aim>(); }
        public Death Death { get => GetComponent<Death>(); }
        public LoseLife LoseLife { get => GetComponent<LoseLife>(); }
        public Shoot Shoot { get => GetComponent<Shoot>(); }
        #endregion

        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _camera = FindObjectOfType<Camera>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    LoseLife.LoseLifePlayer(other.gameObject.GetComponent<Enemy>().GetDefaultDamage());
                    other.gameObject.GetComponent<Enemy>().Death();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.tag)
            {
                case "Shot":
                    LoseLife.LoseLifePlayer(other.GetComponent<Shot>().GetDefaultDamage());
                    other.GetComponent<Shot>().DestroyShot_2();
                    break;
            }
        }

        public void OnDeviceChange(PlayerInput playerInput)
        {
            _isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
        }
    }
}