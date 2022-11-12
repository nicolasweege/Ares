using UnityEngine;
using System.Threading.Tasks;

public class ErosFirstStageState : ErosBaseState {
    private float _timeToFirstWaveShoot = 0.25f;
    private float _firstWaveShootTimer;
    private float _timeToSecondWaveShoot = 0.25f;
    private float _secondWaveShootTimer;
    private bool _isFirstWaveFinished = false;

    public override void EnterState(ErosController context) {
        context.RotateComponent.Rotation = new Vector3(0, 0, 300);
        SwitchStateTimer(5000, context);
    }

    public override void UpdateState(ErosController context) {
        if (PlayerController.Instance == null || context == null) return;

        context.transform.position = Vector2.SmoothDamp(context.transform.position, PlayerController.Instance.transform.position, ref context.Velocity, 2.5f);
        HandleAttack(context);
    }

    private void HandleAttack(ErosController context) {
        if (!_isFirstWaveFinished) {
            _firstWaveShootTimer -= Time.deltaTime;
            if (_firstWaveShootTimer <= 0f) {
                for (int i = 0; i < context.FirstStageBulletDirs_1.Count; i++) {
                    CreateBullet(context.transform, context.FirstStageBullet, context.FirstStageBulletDirs_1[i]);
                }

                SoundManager.PlaySound(SoundManager.Sound.ErosShoot_1, context.transform.position, 0.5f);
                _isFirstWaveFinished = true;
                _firstWaveShootTimer = _timeToFirstWaveShoot;
            }
        }

        if (_isFirstWaveFinished) {
            _secondWaveShootTimer -= Time.deltaTime;
            if (_secondWaveShootTimer <= 0f) {
                for (int i = 0; i < context.FirstStageBulletDirs_2.Count; i++) {
                    CreateBullet(context.transform, context.FirstStageBullet, context.FirstStageBulletDirs_2[i]);
                }

                SoundManager.PlaySound(SoundManager.Sound.ErosShoot_1, context.transform.position, 0.5f);
                _isFirstWaveFinished = false;
                _secondWaveShootTimer = _timeToSecondWaveShoot;
            }
        }
    }

    private void CreateBullet(Transform startingPos, GameObject bullet, Transform bulletDir) {
        var bulletInst = Object.Instantiate(bullet, startingPos.position, Quaternion.identity);
        Vector2 _bulletDir = (bulletDir.position - bulletInst.transform.position).normalized;
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(_bulletDir.x, _bulletDir.y);
    }

    private async void SwitchStateTimer(int delay, ErosController context) {
        await Task.Delay(delay);
        context.SwitchState(context.SecondStageState);
    }
}