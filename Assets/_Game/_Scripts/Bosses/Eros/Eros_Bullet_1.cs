using UnityEngine;

public class Eros_Bullet_1 : MonoBehaviour {
    public float Speed;
    public int Damage = 1;
    public GameObject DamageAnimation;
    public Vector3 Direction;
    public bool IgnoreArenaColliders;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        transform.position += Direction * Time.deltaTime * Speed; // move bullet

        if (Direction.x == 1 && transform.position.x >= 0) IgnoreArenaColliders = false;

        if (Direction.x == -1 && transform.position.x <= 0) IgnoreArenaColliders = false;

        if (Eros.Instance.CurrentState != Eros.Instance.SecondState)
        {
            // destroy bullet outside the screen
            if (transform.position.x > 15f || transform.position.x < -15f) Destroy(gameObject);
            if (transform.position.y > 10f || transform.position.y < -10f) Destroy(gameObject);
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(DamageAnimation, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Satellite":
            case "ArenaCollider":
                if (!IgnoreArenaColliders) DestroyBullet();
                break;

            case "PlayerMainShip": {
                    if (Player.Instance.CanTakeDamage) {
                        if (!Player.Instance.IsDashing) DestroyBullet();
                    }
                }
                break;
        }
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}