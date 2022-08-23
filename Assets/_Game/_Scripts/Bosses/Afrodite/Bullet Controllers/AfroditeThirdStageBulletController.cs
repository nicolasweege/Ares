using UnityEngine;

public class AfroditeThirdStageBulletController : BulletBase
{
    [SerializeField] private Transform _damageAnimSpawnPoint;

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        _destroyVisibleBulletTimer = _timeToDestroyVisibleBullet;
    }

    private void Update()
    {
        MoveProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Satellite"))
            DestroyBullet();

        if (other.CompareTag("ArenaCollider"))
            DestroyBullet();
    }

    public override void DestroyBullet()
    {
        Instantiate(_damageAnim, _damageAnimSpawnPoint.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void MoveProjectile()
    {
        transform.position += _direction * Time.deltaTime * _speed;
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}