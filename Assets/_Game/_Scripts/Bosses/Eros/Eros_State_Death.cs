using UnityEngine;
using System.Threading.Tasks;

public class Eros_State_Death : Eros_State {
    private bool _canScreenShake = false;

    public override void EnterState(Eros context) {
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

    public override void UpdateState(Eros context) {}

    private async void HandleScreenShake(int millisecondsDelay, Eros context) {
        while (_canScreenShake) {
            await Task.Delay(millisecondsDelay);
            CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
        }
    }
}