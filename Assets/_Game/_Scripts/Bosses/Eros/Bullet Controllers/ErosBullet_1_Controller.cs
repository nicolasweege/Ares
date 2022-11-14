using System;
using UnityEngine;

public class ErosBullet_1_Controller : BulletBase {
    [NonSerialized] public bool IgnoreArenaColliders = false;

    protected override void Awake() {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        MoveBullet();

        if (_direction.x == 1 && transform.position.x >= 0)
        {
            IgnoreArenaColliders = false;
        }

        if (_direction.x == -1 && transform.position.x <= 0)
        {
            IgnoreArenaColliders = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Satellite":
            case "ArenaCollider":
                if (!IgnoreArenaColliders) DestroyBullet();
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