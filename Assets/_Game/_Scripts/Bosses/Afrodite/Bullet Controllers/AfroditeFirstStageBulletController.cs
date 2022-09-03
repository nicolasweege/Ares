using UnityEngine;

public class AfroditeFirstStageBulletController : BulletBase {
    // [SerializeField] private float _stopingDist;
    // private bool _isOnStopingDist = false;

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
            case "ArenaCollider":
                DestroyBullet();
                break;
        }
    }

    // talvez eu possa sobreescrever o metodo MoveBullet do BulletBase, ao inves de criar um novo metodo pra isso

    // private void MoveProjectile() {
    //     Script de projetil que segue o player (pode ser usado em outras ocasioes)
    //     var playerPos = PlayerController.Instance.transform.position;

    //     if (Vector2.Distance(transform.position, playerPos) < _stopingDist)
    //         _isOnStopingDist = true;

    //     if (Vector2.Distance(transform.position, playerPos) >= _stopingDist && !_isOnStopingDist)
    //     {
    //         Vector2 bulletDir = PlayerController.Instance.transform.position - transform.position;
    //         bulletDir.Normalize();
    //         float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
    //         transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
    //         // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, bulletAngle), AfroditeController.Instance.FirstStageProjectileTurnSpeed * Time.deltaTime);
    //         _direction = new Vector3(bulletDir.x, bulletDir.y);
    //         transform.position += _direction * Time.deltaTime * _speed;
    //     }

    //     if (_isOnStopingDist)
    //         transform.position += _direction * Time.deltaTime * _speed;
    // }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}