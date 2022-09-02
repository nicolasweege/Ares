using System.Threading.Tasks;

public class AfroditeFourthStageStateSatelliteIdleState : AfroditeFourthStageStateSatelliteBaseState
{
    private int _timeToSwitchState = 2000;

    public override void EnterState(AfroditeFourthStageSatelliteController context) {
        SwitchStateTimer(_timeToSwitchState, context);
    }

    public override void UpdateState(AfroditeFourthStageSatelliteController context) {}

    private async void SwitchStateTimer(int millisecondsDelay, AfroditeFourthStageSatelliteController context) {
        await Task.Delay(millisecondsDelay);
        context.SwitchState(context.ShootingState);
    }
}