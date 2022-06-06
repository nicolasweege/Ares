using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseLife : MonoBehaviour
{
    private PlayerController _playerScript;

    private void Start() => _playerScript = GetComponent<PlayerController>();

    public int PlayerLoseLife(int damage) => _playerScript.Health -= damage;
}