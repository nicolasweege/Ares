using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPlayer
{
    public class LoseLife : MonoBehaviour
    {
        private Player _player;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }

        public int LoseLifePlayer(int damage)
        {
            return _player.Health -= damage;
        }
    }
}