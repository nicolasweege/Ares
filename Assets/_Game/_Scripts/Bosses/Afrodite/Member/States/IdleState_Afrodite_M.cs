using System.Threading.Tasks;

public class IdleState_Afrodite_M : BaseState_Afrodite_M {
    private int _timeToSwitchState = 2000;

    public override void EnterState(AfroditeMemberController context) {
        SwitchStateTimer(_timeToSwitchState, context);
    }

    public override void UpdateState(AfroditeMemberController context) {}

    private async void SwitchStateTimer(int millisecondsDelay, AfroditeMemberController context) {
        await Task.Delay(millisecondsDelay);
        context.SwitchState(context.ShootingState);
    }
}