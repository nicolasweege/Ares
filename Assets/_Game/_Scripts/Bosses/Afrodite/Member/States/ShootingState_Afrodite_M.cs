using UnityEngine;
using System.Threading.Tasks;

public class ShootingState_Afrodite_M : BaseState_Afrodite_M {
    private int _timeToShoot = 500;
    private bool _canShoot = false;

    public override void EnterState(AfroditeMemberController context) {
        _canShoot = true;
        HandleAttack(context);
    }

    public override void UpdateState(AfroditeMemberController context) {}

    private async void HandleAttack(AfroditeMemberController context) {
        while (_canShoot) {
            await Task.Delay(_timeToShoot);
            for (int i = 0; i < context.ShootDirections.Count; i++) {
                if (context == null)
                    return;
                    
                GenerateBullet(context.transform, context.Bullet, context.ShootDirections[i], context);
                SoundManager.PlaySound(SoundManager.Sound.AfroditeThirdStageShoot, context.transform.position, 0.1f);
            }
        }
    }

    private void GenerateBullet(Transform bulletStartingPos, GameObject bulletPrefab, Transform bulletDir, AfroditeMemberController context) {
        Object.Instantiate(context.ShootAnim, context.transform.position, Quaternion.identity);
        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        Vector2 _bulletDir = bulletDir.position - bulletInst.transform.position;
        _bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(_bulletDir.y, _bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(_bulletDir.x, _bulletDir.y);
    }
}