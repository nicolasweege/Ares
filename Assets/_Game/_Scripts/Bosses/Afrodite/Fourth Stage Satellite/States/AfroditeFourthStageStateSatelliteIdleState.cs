using System.Threading.Tasks;

public class AfroditeFourthStageStateSatelliteIdleState : AfroditeFourthStageStateSatelliteBaseState
{
    private int _timeToSwitchState = 2000;

    public override void EnterState(AfroditeFourthStageSatelliteController context) {
        SwitchStateTimer(context);
    }

    public override void UpdateState(AfroditeFourthStageSatelliteController context) {}

    private async void SwitchStateTimer(AfroditeFourthStageSatelliteController context) {
        await Task.Delay(_timeToSwitchState);
        context.SwitchState(context.ShootingState);
    }
}