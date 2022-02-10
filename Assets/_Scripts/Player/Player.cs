using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private GameObject shot;
    [SerializeField] private Transform shotStartPosition;
    [SerializeField] private GameObject damageAnimation;
    [SerializeField] private GameObject deathAnimation;
    private new Rigidbody2D rigidbody2D;
    private PlayerInputActions playerInputActions;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Instantiate(shot, shotStartPosition.position, shotStartPosition.rotation);
    }

    private void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        transform.position += new Vector3(inputVector.x, inputVector.y, transform.position.z) * Time.deltaTime * speed;
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

        if (health <= 0) Death();
    }

    private int LoseLife(int damage)
    {
        return health -= damage;
    }

    private void Death()
    {
        Destroy(gameObject);
        Instantiate(deathAnimation, transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                LoseLife(other.GetComponent<Enemy>().defaultDamage);
                other.GetComponent<Enemy>().Death();
                break;

            case "Shot":
                LoseLife(other.GetComponent<Shot>().defaultDamage);
                other.GetComponent<Shot>().DestroyShot();
                break;
        }
    }
}
