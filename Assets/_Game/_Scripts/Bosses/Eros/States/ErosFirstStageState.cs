using UnityEngine;

public class ErosFirstStageState : ErosBaseState {
    private float _timeToShoot = 1;
    private float _shootTimer;

    public override void EnterState(ErosController context) {
        context.RotateComponent.Rotation = new Vector3(0, 0, 300);
    }

    public override void UpdateState(ErosController context) {
        if (PlayerController.Instance == null || context == null) {
            return;
        }

        // movement
        context.transform.position = Vector2.SmoothDamp(context.transform.position, PlayerController.Instance.transform.position, ref context.Velocity, 1);
        
        HandleAttack(context);
    }

    // private void HandleAttack(AfroditeController context) {
    //     if (!_isFirstWaveFinished) {
    //         _firstWaveShootTimer -= Time.deltaTime;
    //         if (_firstWaveShootTimer <= 0f) {
    //             for (int i = 0; i < context.ThirdStageFirstWaveShootDirections.Count; i++) {
    //                 GenerateBullet(context.transform, context.ThirdStageProjectile, context.ThirdStageFirstWaveShootDirections[i], context);
    //             }

    //             SoundManager.PlaySound(SoundManager.Sound.AfroditeThirdStageShoot, context.transform.position, 0.3f);
    //             _isFirstWaveFinished = true;
    //             _firstWaveShootTimer = _timeToFirstWaveShoot;
    //         }
    //     }

    //     if (_isFirstWaveFinished) {
    //         _secondWaveShootTimer -= Time.deltaTime;
    //         if (_secondWaveShootTimer <= 0f) {
    //             for (int i = 0; i < context.ThirdStageSecondWaveShootDirections.Count; i++) {
    //                 GenerateBullet(context.transform, context.ThirdStageProjectile, context.ThirdStageSecondWaveShootDirections[i], context);
    //             }

    //             SoundManager.PlaySound(SoundManager.Sound.AfroditeThirdStageShoot, context.transform.position, 0.3f);
    //             _isFirstWaveFinished = false;
    //             _secondWaveShootTimer = _timeToSecondWaveShoot;
    //         }
    //     }
    // }

    private void HandleAttack(ErosController context) {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0) {
            for (int i = 0; i < context.FirstStageBulletDirs.Count; i++) {
                CreateBullet(context.transform, context.FirstStageBullet, context.FirstStageBulletDirs[i]);
            }

            _shootTimer = _timeToShoot;
        }
    }

    private void CreateBullet(Transform startingPos, GameObject bullet, Transform bulletDir) {
        var bulletInst = Object.Instantiate(bullet, startingPos.position, Quaternion.identity);
        Vector2 _bulletDir = (bulletDir.position - bulletInst.transform.position).normalized;
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(_bulletDir.x, _bulletDir.y);
    }
}