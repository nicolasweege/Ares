using UnityEngine;
using System.Threading.Tasks;

public class AfroditeFourthStageStateSatelliteIdleState : AfroditeFourthStageStateSatelliteBaseState
{
    private int _timeToSwitchState = 2000;

    public override void EnterState(AfroditeFourthStageSatelliteController context) {
        context.RotateComponent.Rotation = new Vector3(0, 0, 100);
        SwitchStateTimer(_timeToSwitchState, context);
    }

    public override void UpdateState(AfroditeFourthStageSatelliteController context) {}

    private async void SwitchStateTimer(int millisecondsDelay, AfroditeFourthStageSatelliteController context) {
        await Task.Delay(millisecondsDelay);
        context.RotateComponent.Rotation = new Vector3(0, 0, 0);
        context.SwitchState(context.ShootingState);
    }
}