using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private GameObject playerShotPrefab;
    private Rigidbody2D playerRb2D;
    private PlayerInputActions playerInputActions;

    private void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void Update()
    {

    }
}
