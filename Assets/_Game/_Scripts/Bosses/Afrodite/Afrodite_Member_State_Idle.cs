using System.Threading.Tasks;

public class Afrodite_Member_State_Idle : Afrodite_Member_State
{
    private int _timeToSwitchState = 2000;

    public override void EnterState(Afrodite_Member context) {
        SwitchStateTimer(_timeToSwitchState, context);
    }

    public override void UpdateState(Afrodite_Member context) {}

    private async void SwitchStateTimer(int millisecondsDelay, Afrodite_Member context) {
        await Task.Delay(millisecondsDelay);
        context.SwitchState(context.ShootState);
    }
}