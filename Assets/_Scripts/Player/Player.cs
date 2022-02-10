using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private GameObject playerShot;
    [SerializeField] private Transform shotStartPosition;
    [SerializeField] private GameObject damageAnimation;
    private Rigidbody2D playerRb2D;
    private PlayerInputActions playerInputActions;

    private void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Instantiate(playerShot, shotStartPosition.position, shotStartPosition.rotation);
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

        if (playerHealth <= 0) Death();
    }

    private int LoseLife(int damage)
    {
        return playerHealth -= damage;
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                LoseLife(other.GetComponent<Enemy>().defaultDamage);
                other.GetComponent<Enemy>().Death();
                break;

            case "Enemy1Shot":
                Enemy1Shot enemy1ShotScript = other.GetComponent<Enemy1Shot>();
                Transform enemy1ShotTransform = other.GetComponent<Transform>();

                LoseLife(enemy1ShotScript.defaultDamage);
                Instantiate(damageAnimation, enemy1ShotTransform.position, enemy1ShotTransform.rotation);
                Destroy(other.gameObject);
                break;
        }
    }
}
