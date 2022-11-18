using UnityEngine;

public class ErosThirdStageState : ErosBaseState {
    private float _speed = 20f;
    private bool _canAttack = false;
    private Vector3 _attackDir;

    public override void EnterState(ErosController context) {
        // First Teleport
        FunctionTimer.Create(() => context.SpritesGameObject.SetActive(false), 1f, "set sprites off");
        FunctionTimer.Create(() => context.transform.position = context.NullMovePoint.position, 1.1f);

        FunctionTimer.Create(() => {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            if (playerPos.x < 0)
                context.transform.position = context.MovePointLeft.position;
            if (playerPos.x >= 0)
                context.transform.position = context.MovePointRight.position;
        }, 1.5f, "set Eros position");

        FunctionTimer.Create(() => context.SpritesGameObject.SetActive(true), 1.6f, "set sprites on");

        // Attack
        FunctionTimer.Create(() => {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            _attackDir = (playerPos - context.transform.position).normalized;
            _canAttack = true;
        }, 2f, "set and start Eros attack");

        // Last Teleport
        FunctionTimer.Create(() => _canAttack = false, 4.9f, "stop Eros attack");
        FunctionTimer.Create(() => context.SpritesGameObject.SetActive(false), 5f, "set sprites off");
        FunctionTimer.Create(() => context.transform.position = context.NullMovePoint.position, 5.1f);
        FunctionTimer.Create(() => context.transform.position = new Vector3(0, 0, 0), 5.5f, "set Eros position");
        FunctionTimer.Create(() => context.SpritesGameObject.SetActive(true), 5.6f, "set sprites on");
        FunctionTimer.Create(() => context.SwitchState(context.FirstStageState), 6f, "switch state");
    }

    public override void UpdateState(ErosController context) {
        if (PlayerController.Instance == null || context == null)
            return;

        if (_canAttack)
            context.transform.position += _attackDir * Time.deltaTime * _speed;
    }
}