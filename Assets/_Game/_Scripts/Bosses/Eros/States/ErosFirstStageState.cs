using UnityEngine;

public class ErosFirstStageState : ErosBaseState {
    public override void EnterState(ErosController context) {
        context.RotateComponent.Rotation = new Vector3(0, 0, 50);
    }

    public override void UpdateState(ErosController context) {}
}