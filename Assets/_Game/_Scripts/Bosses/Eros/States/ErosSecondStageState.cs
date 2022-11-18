using UnityEngine;

public class ErosSecondStageState : ErosBaseState {
    private bool _canShoot = false;
    private float _timeToFirstWaveShoot = 0.5f;
    private float _firstWaveShootTimer;
    private float _timeToSecondWaveShoot = 0.5f;
    private float _secondWaveShootTimer;
    private bool _isFirstWaveFinished = false;

    public override void EnterState(ErosController context) {
        FunctionTimer.Create(() => context.SwitchState(context.FirstStageState), 12f);
        FunctionTimer.Create(() => _canShoot = true, 0.1f);
        FunctionTimer.Create(() => _canShoot = false, 8f);
        FunctionTimer.Create(() => context.SpritesGameObject.SetActive(true), 11f);

        // Teleport
        FunctionTimer.Create(() => context.SpritesGameObject.SetActive(false), 1.5f);
        FunctionTimer.Create(() => context.transform.position = context.NullMovePoint.position, 1.6f);

        FunctionTimer.Create(() => {
            if (PlayerController.Instance.transform.position.y >= 0)
                context.transform.position = context.MovePointDown.position;

            if (PlayerController.Instance.transform.position.y < 0)
                context.transform.position = context.MovePointUp.position;
        }, 2f);

        FunctionTimer.Create(() => context.SpritesGameObject.SetActive(true), 2.1f);
    }

    public override void UpdateState(ErosController context) {
        if (PlayerController.Instance == null || context == null)
            return;

        if (_canShoot) HandleAttack(context);
    }

    private void HandleAttack(ErosController context) {
        if (!_isFirstWaveFinished)
        {
            _firstWaveShootTimer -= Time.deltaTime;
            if (_firstWaveShootTimer <= 0f)
            {
                float yy = 0f;
                for (int i = 0; i < 10; i++)
                {
                    var bullet = CreateBullet(new Vector3(40, -9 + yy, 0), context.FirstStageBullet, Vector3.left);
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
                    var bullet = CreateBullet(new Vector3(40, -8 + yy, 0), context.FirstStageBullet, Vector3.left);
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
        bulletInst.GetComponent<ErosBullet_1_Controller>().IgnoreArenaColliders = true;

        bulletInst.GetComponent<BulletBase>().Direction = bulletDir;

        float bulletSpeed = bulletInst.GetComponent<ErosBullet_1_Controller>().Speed;
        bulletInst.GetComponent<ErosBullet_1_Controller>().Speed = Random.Range(bulletSpeed, bulletSpeed * 1.5f);

        return bulletInst;
    }
}