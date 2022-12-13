using UnityEngine;

public class Afrodite_Bullet_2 : MonoBehaviour {
    public float Speed;
    public int Damage = 1;
    public GameObject DamageAnimation;
    public Vector3 Direction;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        transform.position += Direction * Time.deltaTime * Speed; // move bullet
    }

    public virtual void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(DamageAnimation, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Satellite":
            case "ArenaCollider":
                DestroyBullet();
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