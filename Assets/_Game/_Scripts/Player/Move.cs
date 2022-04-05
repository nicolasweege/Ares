using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Player _playerController;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _mapGenerator;
    private PlayerInputActions _playerInputActions;

    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 movementInputVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        movementInputVector.Normalize();

        transform.position += new Vector3(movementInputVector.x, movementInputVector.y) * Time.deltaTime * _speed;

        float xx = Mathf.Clamp(transform.position.x, -_mapGenerator.GetComponent<MapGenerator>().GetMapWidth(), _mapGenerator.GetComponent<MapGenerator>().GetMapWidth());
        float yy = Mathf.Clamp(transform.position.y, -_mapGenerator.GetComponent<MapGenerator>().GetMapHight(), _mapGenerator.GetComponent<MapGenerator>().GetMapHight());

        transform.position = new Vector3(xx, yy);
    }
}