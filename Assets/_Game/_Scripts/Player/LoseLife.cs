using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseLife : MonoBehaviour
{
    private Player _playerScript;

    private void Start() => _playerScript = GetComponent<Player>();

    public int PlayerLoseLife(int damage) => _playerScript.Health -= damage;
}