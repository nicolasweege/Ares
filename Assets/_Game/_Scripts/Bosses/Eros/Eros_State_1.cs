using UnityEngine;

public class Eros_State_1 : Eros_State {
    private float _timeToFirstWaveShoot = 0.4f;
    private float _firstWaveShootTimer;
    private float _timeToSecondWaveShoot = 0.4f;
    private float _secondWaveShootTimer;
    private bool _isFirstWaveFinished = false;

    public override void EnterState(Eros context) {
        context.RotateComponent.Rotation = new Vector3(0, 0, 250);
    }

    public override void UpdateState(Eros context) {
        if (Player.Instance == null || context == null) 
            return;

        context.transform.position = Vector2.SmoothDamp(context.transform.position,
                                                        Player.Instance.transform.position, 
                                                        ref context.Velocity, 
                                                        2.5f);

        HandleAttack(context);
    }

    private void HandleAttack(Eros context) {
        if (!_isFirstWaveFinished) {
            _firstWaveShootTimer -= Time.deltaTime;
            if (_firstWaveShootTimer <= 0f) {
                for (int i = 0; i < context.State_1_BulletDirs_1.Count; i++)
                    CreateBullet(context.transform, context.State_1_Bullet, context.State_1_BulletDirs_1[i]);

                Object.Instantiate(context.MainAnimation, context.transform.position, Quaternion.identity);
                SoundManager.PlaySound(SoundManager.Sound.ErosShoot_1, context.transform.position, 0.5f);
                _isFirstWaveFinished = true;
                _firstWaveShootTimer = _timeToFirstWaveShoot;
            }
        }

        if (_isFirstWaveFinished) {
            _secondWaveShootTimer -= Time.deltaTime;
            if (_secondWaveShootTimer <= 0f) {
                for (int i = 0; i < context.State_1_BulletDirs_2.Count; i++)
                    CreateBullet(context.transform, context.State_1_Bullet, context.State_1_BulletDirs_2[i]);

                Object.Instantiate(context.MainAnimation, context.transform.position, Quaternion.identity);
                SoundManager.PlaySound(SoundManager.Sound.ErosShoot_1, context.transform.position, 0.5f);
                _isFirstWaveFinished = false;
                _secondWaveShootTimer = _timeToSecondWaveShoot;
            }
        }
    }

    private void CreateBullet(Transform startingPos, GameObject bullet, Transform bulletDir) {
        var bulletInst = Object.Instantiate(bullet, startingPos.position, Quaternion.identity);
        Vector2 _bulletDir = (bulletDir.position - bulletInst.transform.position).normalized;
        bulletInst.GetComponent<Eros_Bullet_1>().Direction = new Vector3(_bulletDir.x, _bulletDir.y);
    }
}