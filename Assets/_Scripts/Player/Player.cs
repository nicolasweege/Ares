using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private GameObject playerShotPrefab;
    [SerializeField] private Transform shotStartPosition;
    [SerializeField] private GameObject damageAnimationPrefab;
    private Rigidbody2D playerRb2D;
    private PlayerInputActions playerInputActions;

    private void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += Shoot;
    }

    private void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Instantiate(playerShotPrefab, shotStartPosition.position, shotStartPosition.rotation);
    }

    private void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        transform.position += new Vector3(inputVector.x, inputVector.y, transform.position.z) * Time.deltaTime * playerSpeed;
    }

    private void FixedUpdate()
    {
        if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, transform.position.z);
        }
        
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy1Shot":
                Instantiate(damageAnimationPrefab, other.GetComponent<Transform>().position, other.GetComponent<Transform>().rotation);
                Destroy(other.gameObject);
                break;
        }
    }
}
