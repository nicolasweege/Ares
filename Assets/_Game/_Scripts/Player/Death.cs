using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPlayer
{
    public class Death : MonoBehaviour
    {
        [SerializeField] private GameObject _deathAnimation;
        private Player _player;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }

        private void Update()
        {
            if (_player.Health <= 0)
                DeathPlayer();
        }

        public void DeathPlayer()
        {
            Destroy(gameObject);
            Instantiate(_deathAnimation, transform.position, Quaternion.identity);
        }
    }
}