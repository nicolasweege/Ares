using UnityEngine;
using System.Threading.Tasks;

public class AfroditeDeathState : AfroditeBaseState {
    private bool _canScreenShake = false;

    public override void EnterState(AfroditeController context) {
        context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
        context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableFeedbackLaser();
        _canScreenShake = true;
        HandleScreenShake(500, context);
        HandleDeath(2000, context);
    }

    public override void UpdateState(AfroditeController context) {}

    private async void HandleScreenShake(int millisecondsDelay, AfroditeController context) {
        while (_canScreenShake) {
            await Task.Delay(millisecondsDelay);
            CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
        }
    }

    private async void HandleDeath(int millisecondsDelay, AfroditeController context) {
        await Task.Delay(millisecondsDelay);
        _canScreenShake = false;
        Object.Destroy(context.gameObject);
        Object.Instantiate(context.DeathAnim, context.transform.position, Quaternion.identity);
        GameManager.Instance.SetGameState(GameState.WinState);
    }
}