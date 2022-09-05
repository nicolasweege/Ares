using System.Threading.Tasks;

public class ErosIdleState : ErosBaseState {
    public override void EnterState(ErosController context) {
        SwitchStateTimer(1000, context);
    }

    public override void UpdateState(ErosController context) {}

    private async void SwitchStateTimer(int delay, ErosController context) {
        await Task.Delay(delay);
        context.SwitchState(context.FirstStageState);
    }
}