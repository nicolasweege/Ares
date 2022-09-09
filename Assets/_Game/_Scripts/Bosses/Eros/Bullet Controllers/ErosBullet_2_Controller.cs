using UnityEngine;

public class ErosBullet_2_Controller : BulletBase {
    [SerializeField] private float _stopingDist;
    private bool _isOnStopingDist = false;

    protected override void Awake() {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update() {
        MoveBullet();
    }

    protected override void MoveBullet() {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < _stopingDist) {
            _isOnStopingDist = true;
        }

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) >= _stopingDist && !_isOnStopingDist) {
            Vector2 bulletDir = (PlayerController.Instance.transform.position - transform.position).normalized;
            _direction = new Vector3(bulletDir.x, bulletDir.y);
            transform.position += _direction * Time.deltaTime * _speed;
        }

        if (_isOnStopingDist) {
            transform.position += _direction * Time.deltaTime * _speed;
        }
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