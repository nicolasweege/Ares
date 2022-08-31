using UnityEngine;
using System.Threading.Tasks;

public class AfroditeDeathState : AfroditeBaseState
{
    private int _timeToDeathAnim = 2000;
    private int _timeToShakeScreen = 500;
    private bool _canScreenShake = false;

    public override void EnterState(AfroditeController context) {
        context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
        _canScreenShake = true;
        HandleScreenShake(_timeToShakeScreen, context);
        HandleDeath(_timeToDeathAnim, context);
    }

    public override void UpdateState(AfroditeController context) {}

    private async void HandleDeath(int millisecondsDelay, AfroditeController context) {
        await Task.Delay(millisecondsDelay);
        _canScreenShake = false;
        Object.Destroy(context.gameObject);
        Object.Instantiate(context.DeathAnim, context.transform.position, Quaternion.identity);
        context.SwitchState(context.IdleState);
        GameManager.Instance.HandleWonLevel();
    }

    private async void HandleScreenShake(int millisecondsDelay, AfroditeController context) {
        while (_canScreenShake) {
            await Task.Delay(millisecondsDelay);
            CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
        }
    }
}