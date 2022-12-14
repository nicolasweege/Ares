using UnityEngine;

public class Player_Bullet : MonoBehaviour {
    public float Speed;
    public int Damage = 1;
    public GameObject DamageAnimation;
    public Vector3 Direction;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        transform.position += Direction * Time.deltaTime * Speed; // move bullet

        // destroy bullet outside the screen
        if (transform.position.x > 15f || transform.position.x < -15f) Destroy(gameObject);
        if (transform.position.y > 10f || transform.position.y < -10f) Destroy(gameObject);
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(DamageAnimation, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Satellite":
            case "SatelliteLaserCollider":
            case "ArenaCollider":
                DestroyBullet();
                break;

            case "AfroditeMainShip":
                if (Afrodite.Instance.CurrentState != Afrodite.Instance.DeathState) {
                    Afrodite.Instance.TakeDamage(Damage);
                    DestroyBullet();
                }
                break;
            
            case "Eros":
                if (Eros.Instance.CurrentState != Eros.Instance.DeathState) {
                    Eros.Instance.TakeDamage(Damage);
                    DestroyBullet();
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