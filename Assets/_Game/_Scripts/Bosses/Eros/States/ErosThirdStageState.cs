using UnityEngine;

public class ErosThirdStageState : ErosBaseState {
    private float _speed = 30f;
    private bool _canAttack = false;
    private Vector3 _attackDir;

    public override void EnterState(ErosController context) {
        // First Teleport
        FunctionTimer.Create(() => context.transform.position = context.NullMovePoint.position, 0.3f, "Set Eros null position");

        FunctionTimer.Create(() => {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            if (playerPos.x < 0)
                context.transform.position = context.MovePointLeft.position;
            if (playerPos.x >= 0)
                context.transform.position = context.MovePointRight.position;
        }, 0.5f, "Set Eros LEFT or RIGHT position");

        // Attack
        FunctionTimer.Create(() => {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            _attackDir = (playerPos - context.transform.position).normalized;
        }, 0.8f, "Get Player direction");

        FunctionTimer.Create(() => _canAttack = true, 0.9f, "Set _canAttack to true");

        // Last Teleport
        FunctionTimer.Create(() => _canAttack = false, 3f, "Set _canAttack to false");
        FunctionTimer.Create(() => context.transform.position = context.NullMovePoint.position, 3.1f, "Set Eros null position");
        FunctionTimer.Create(() => context.transform.position = new Vector3(0, 0, 0), 3.2f, "Set Eros center position");
        FunctionTimer.Create(() => context.SwitchState(context.FirstStageState), 3.5f, "Switch state");
    }

    public override void UpdateState(ErosController context) {
        if (PlayerController.Instance == null || context == null)
            return;

        if (_canAttack)
            context.transform.position += _attackDir * Time.deltaTime * _speed;
    }
}