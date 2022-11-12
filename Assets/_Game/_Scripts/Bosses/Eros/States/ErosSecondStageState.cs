using UnityEngine;
using System.Threading.Tasks;

public class ErosSecondStageState : ErosBaseState {
    private float _timeToShoot = 1;
    private float _shootTimer;

    public override void EnterState(ErosController context) {
        Teleport(500, 500, context);
        // SwitchStateTimer(5000, context);
    }

    public override void UpdateState(ErosController context) {
        if (PlayerController.Instance == null || context == null) return;

        // context.transform.position = Vector2.SmoothDamp(context.transform.position, PlayerController.Instance.transform.position, ref context.Velocity, 2.5f);
        // HandleAttack(context);

    }

    private void HandleAttack(ErosController context) {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f) {
            CreateBullet(context.transform, context.SecondStageBullet, PlayerController.Instance.transform);
            SoundManager.PlaySound(SoundManager.Sound.ErosShoot_1, context.transform.position, 0.5f);
            _shootTimer = _timeToShoot;
        }
    }

    private void CreateBullet(Transform startingPos, GameObject bullet, Transform bulletDir) {
        var bulletInst = Object.Instantiate(bullet, startingPos.position, Quaternion.identity);
        Vector2 _bulletDir = (bulletDir.position - bulletInst.transform.position).normalized;
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(_bulletDir.x, _bulletDir.y);
    }

    private async void Teleport(int firstTeleportDelay, int secondTeleportDelay, ErosController context)
    {
        await Task.Delay(firstTeleportDelay);
        context.SpritesGameObject.SetActive(false);
        context.transform.position = context.NullMovePoint.position;

        await Task.Delay(secondTeleportDelay);
        if (PlayerController.Instance.transform.position.y >= 0)
            context.transform.position = context.MovePointUp.position;

        if (PlayerController.Instance.transform.position.y < 0)
            context.transform.position = context.MovePointDown.position;
        
        context.SpritesGameObject.SetActive(true);

        await Task.Delay(secondTeleportDelay);
        context.SwitchState(context.FirstStageState);
    }

    private async void SwitchStateTimer(int delay, ErosController context) {
        await Task.Delay(delay);
        context.SwitchState(context.FirstStageState);
    }
}