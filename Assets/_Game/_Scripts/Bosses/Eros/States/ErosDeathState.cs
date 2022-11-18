using UnityEngine;
using System.Threading.Tasks;

public class ErosDeathState : ErosBaseState {
    private bool _canScreenShake = false;

    public override void EnterState(ErosController context) {
        _canScreenShake = true;
        context.RotateComponent.Rotation = Vector3.zero;
        HandleScreenShake(500, context);

        FunctionTimer.Create(() => {
            _canScreenShake = false;
            Object.Destroy(context.gameObject);
            Object.Instantiate(context.MainAnimation, context.transform.position, Quaternion.identity);
            GameManager.Instance.SetGameState(GameState.WinState);
        }, 2f, "Handle Eros death");
    }

    public override void UpdateState(ErosController context) {}

    private async void HandleScreenShake(int millisecondsDelay, ErosController context) {
        while (_canScreenShake) {
            await Task.Delay(millisecondsDelay);
            CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
        }
    }
}