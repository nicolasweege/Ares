using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPlayer
{
    public class Aim : MonoBehaviour
    {
        private Camera _camera;
        private Rigidbody2D _rb2D;

        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _camera = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            AimPlayer();
        }

        public Vector2 AimPlayer()
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
    }
}