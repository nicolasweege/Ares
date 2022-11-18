using UnityEngine;

public class ShootingState_Afrodite_M : BaseState_Afrodite_M {
    // private int _timeToShoot = 500;
    private bool _canShoot = true;
    private float _timeToShoot = 0.5f;
    private float _shootTimer;

    public override void EnterState(AfroditeMemberController context) {
        // _canShoot = true;
        // HandleAttack(context);
    }

    public override void UpdateState(AfroditeMemberController context) {
        if (PlayerController.Instance == null || context == null)
            return;

        if (_canShoot) {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0) {
                for (int i = 0; i < context.ShootDirections.Count; i++) {
                    if (context == null)
                        return;

                    GenerateBullet(context.transform, context.Bullet, context.ShootDirections[i], context);
                    SoundManager.PlaySound(SoundManager.Sound.AfroditeThirdStageShoot, context.transform.position, 0.1f);
                }
                _shootTimer = _timeToShoot;
            }
        }
    }
    /*
    private async void HandleAttack(AfroditeMemberController context) {
        while (_canShoot) {
            await Task.Delay(_timeToShoot1);
            for (int i = 0; i < context.ShootDirections.Count; i++) {
                if (context == null)
                    return;
                    
                GenerateBullet(context.transform, context.Bullet, context.ShootDirections[i], context);
                SoundManager.PlaySound(SoundManager.Sound.AfroditeThirdStageShoot, context.transform.position, 0.1f);
            }
        }
    }
    */
    private void GenerateBullet(Transform bulletStartingPos, GameObject bulletPrefab, Transform bulletDir, AfroditeMemberController context) {
        Object.Instantiate(context.ShootAnim, context.transform.position, Quaternion.identity);
        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        Vector2 _bulletDir = (bulletDir.position - bulletInst.transform.position).normalized;
        float bulletAngle = Mathf.Atan2(_bulletDir.y, _bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(_bulletDir.x, _bulletDir.y);
    }
}