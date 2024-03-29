using UnityEngine;

public class Eros_State_2 : Eros_State {
    private bool _canShoot = true;
    private float _timeToFirstWaveShoot = 1f;
    private float _firstWaveShootTimer;
    private float _timeToSecondWaveShoot = 1f;
    private float _secondWaveShootTimer;
    private bool _isFirstWaveFinished = false;
    private Vector3 _bulletsDirection;
    private float _bulletsXSpawnPosition;
    private int _randomNumber;

    public override void EnterState(Eros context) {
        // Set bullets' direction and X spawn position
        _randomNumber = Random.Range(0, 2);
        if (_randomNumber != 0) {
            _bulletsDirection = Vector3.left;
            _bulletsXSpawnPosition = 40;
        }
        if (_randomNumber == 0) {
            _bulletsDirection = Vector3.right;
            _bulletsXSpawnPosition = -40;
        }

        FunctionTimer.Create(() => _canShoot = true, 0.7f, "Set _canShoot to true");

        // Teleport
        FunctionTimer.Create(() => {
            var animPos = context.transform.position;
            context.transform.position = context.CenterMovePoint.position;
            Object.Instantiate(context.MainAnimation, animPos, Quaternion.identity);
        }, 1f, "Set Eros null position");

        FunctionTimer.Create(() => {
            if (Player.Instance.transform.position.y >= 0) {
                Object.Instantiate(context.MainAnimation, context.MovePointDown.position, Quaternion.identity);
                context.transform.position = context.MovePointDown.position;
            }

            if (Player.Instance.transform.position.y < 0) {
                Object.Instantiate(context.MainAnimation, context.MovePointUp.position, Quaternion.identity);
                context.transform.position = context.MovePointUp.position;
            }
        }, 1.5f, "Set Eros DOWN or UP position");

        FunctionTimer.Create(() => _canShoot = false, 8f, "Set _canShoot to false");

        FunctionTimer.Create(() => {
            var animPos = context.transform.position;
            context.transform.position = context.CenterMovePoint.position;
            Object.Instantiate(context.MainAnimation, animPos, Quaternion.identity);
        }, 11.5f, "Set Eros null position");

        FunctionTimer.Create(() => {
            Object.Instantiate(context.MainAnimation, new Vector3(0, 0, 0), Quaternion.identity);
            context.transform.position = new Vector3(0, 0, 0);
        }, 12f, "Set Eros center position");

        FunctionTimer.Create(() => context.SwitchState(context.FirstState), 12.5f, "Switch state");
    }

    public override void UpdateState(Eros context) {
        if (Player.Instance == null || context == null)
            return;

        if (_canShoot)
            HandleAttack(context);
    }

    private void HandleAttack(Eros context) {
        if (!_isFirstWaveFinished)
        {
            _firstWaveShootTimer -= Time.deltaTime;
            if (_firstWaveShootTimer <= 0f)
            {
                float yy = 0f;
                for (int i = 0; i < 10; i++)
                {
                    var bullet = CreateBullet(new Vector3(_bulletsXSpawnPosition, -9 + yy, 0), context.State_1_Bullet, _bulletsDirection);
                    yy += 2;
                }

                // SoundManager.PlaySound(SoundManager.Sound.ErosShoot_1, 0.5f);
                _isFirstWaveFinished = true;
                _firstWaveShootTimer = _timeToFirstWaveShoot;
            }
        }

        if (_isFirstWaveFinished)
        {
            _secondWaveShootTimer -= Time.deltaTime;
            if (_secondWaveShootTimer <= 0f)
            {
                float yy = 0f;
                for (int i = 0; i < 9; i++)
                {
                    var bullet = CreateBullet(new Vector3(_bulletsXSpawnPosition, -8 + yy, 0), context.State_1_Bullet, _bulletsDirection);
                    yy += 2;
                }

                // SoundManager.PlaySound(SoundManager.Sound.ErosShoot_1, 0.5f);
                _isFirstWaveFinished = false;
                _secondWaveShootTimer = _timeToSecondWaveShoot;
            }
        }
    }

    private GameObject CreateBullet(Vector3 startingPos, GameObject bullet, Vector2 bulletDir) {
        var bulletInst = Object.Instantiate(bullet, startingPos, Quaternion.identity);
        bulletInst.GetComponent<Eros_Bullet_1>().IgnoreArenaColliders = true;

        bulletInst.GetComponent<Eros_Bullet_1>().Direction = bulletDir;

        float bulletSpeed = bulletInst.GetComponent<Eros_Bullet_1>().Speed;
        bulletInst.GetComponent<Eros_Bullet_1>().Speed = Random.Range(bulletSpeed, bulletSpeed * 1.5f);

        return bulletInst;
    }
}