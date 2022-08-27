using UnityEngine;
using System.Threading.Tasks;

public class AfroditeFourthStageStateSatelliteShootingState : AfroditeFourthStageStateSatelliteBaseState
{
    private int _timeToShoot = 1000;
    private bool _canShoot = false;

    public override void EnterState(AfroditeFourthStageSatelliteController context) {
        _canShoot = true;
        HandleAttack(context);
    }

    public override void UpdateState(AfroditeFourthStageSatelliteController context) {}

    private async void HandleAttack(AfroditeFourthStageSatelliteController context) {
        while (_canShoot) {
            await Task.Delay(_timeToShoot);
            for (int i = 0; i < context.ShootDirections.Count; i++) {
                if (context == null)
                    return;
                    
                GenerateBullet(context.transform, context.Projectile, context.ShootDirections[i], context);
                SoundManager.PlaySound(SoundManager.Sound.AfroditeThirdStageShoot, context.transform.position, 0.1f);
            }
        }
    }

    private void GenerateBullet(Transform bulletStartingPos, GameObject bulletPrefab, Transform projectileDir, AfroditeFourthStageSatelliteController context)
    {
        Object.Instantiate(context.ShootAnim, context.transform.position, Quaternion.identity);
        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        Vector2 bulletDir = projectileDir.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }
}