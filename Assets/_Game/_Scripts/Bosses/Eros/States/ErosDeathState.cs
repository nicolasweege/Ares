using UnityEngine;
using System.Threading.Tasks;

public class ErosDeathState : ErosBaseState {
    private bool _canScreenShake = false;

    public override void EnterState(ErosController context) {
        _canScreenShake = true;
        context.RotateComponent.Rotation = Vector3.zero;
        HandleScreenShake(500, context);
        HandleDeath(2000, context);
    }

    public override void UpdateState(ErosController context) {}

    private async void HandleScreenShake(int millisecondsDelay, ErosController context) {
        while (_canScreenShake) {
            await Task.Delay(millisecondsDelay);
            CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
        }
    }

    private async void HandleDeath(int millisecondsDelay, ErosController context) {
        await Task.Delay(millisecondsDelay);
        _canScreenShake = false;
        Object.Destroy(context.gameObject);
        Object.Instantiate(context.DeathAnim, context.transform.position, Quaternion.identity);
        GameManager.Instance.SetGameState(GameState.WinState);
    }
}