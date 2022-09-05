using UnityEngine;
using System.Threading.Tasks;

public class ErosSecondStageState : ErosBaseState {
    public override void EnterState(ErosController context) {
        SwitchStateTimer(3000, context);
    }

    public override void UpdateState(ErosController context) {
        if (PlayerController.Instance == null || context == null) {
            return;
        }

        context.transform.position = Vector2.SmoothDamp(context.transform.position, PlayerController.Instance.transform.position, ref context.Velocity, 2.5f);
    }

    private async void SwitchStateTimer(int delay, ErosController context) {
        await Task.Delay(delay);
        context.SwitchState(context.FirstStageState);
    }
}