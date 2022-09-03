using UnityEngine;

public class PlayerBulletController : BulletBase {
    protected override void Awake() {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        MoveBullet();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Satellite":
            case "SatelliteLaserCollider":
            case "ArenaCollider":
                DestroyBullet();
                break;

            case "AfroditeMainShip":
                if (AfroditeController.Instance.CurrentState != AfroditeController.Instance.DeathState) {
                    AfroditeController.Instance.TakeDamage(_defaultDamage);
                    DestroyBullet();
                }
                break;
            
            case "Eros":
                if (ErosController.Instance.CurrentState != ErosController.Instance.DeathState) {
                    ErosController.Instance.TakeDamage(_defaultDamage);
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