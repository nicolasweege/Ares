using System.Threading.Tasks;

public class Eros_State_Idle : Eros_State {
    public override void EnterState(Eros context) {
        SwitchStateTimer(1000, context);
    }

    public override void UpdateState(Eros context) {}

    private async void SwitchStateTimer(int delay, Eros context) {
        await Task.Delay(delay);
        context.SwitchState(context.FirstState);
    }
}