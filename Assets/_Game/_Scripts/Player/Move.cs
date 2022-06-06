using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _mapGeneratorPf;
    private PlayerController _playerScript;

    private void Start()
    {
        _playerScript = GetComponent<PlayerController>();
        _playerScript.PlayerInputActions = new PlayerInputActions();
        _playerScript.PlayerInputActions.Player.Enable();
    }

    private void Update() => PlayerMove();

    public void PlayerMove()
    {
        Vector2 movementInputVector = _playerScript.PlayerInputActions.Player.Movement.ReadValue<Vector2>();
        movementInputVector.Normalize();

        transform.position += new Vector3(movementInputVector.x, movementInputVector.y) * Time.deltaTime * _speed;

        float xx = Mathf.Clamp(transform.position.x, -_mapGeneratorPf.GetComponent<MapGenerator>().MapWidth, _mapGeneratorPf.GetComponent<MapGenerator>().MapWidth);
        float yy = Mathf.Clamp(transform.position.y, -_mapGeneratorPf.GetComponent<MapGenerator>().MapHight, _mapGeneratorPf.GetComponent<MapGenerator>().MapHight);
        transform.position = new Vector3(xx, yy);
    }
}