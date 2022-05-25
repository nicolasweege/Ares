using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    public class Enemy_1_Map_1 : Enemy
    {
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    }
}