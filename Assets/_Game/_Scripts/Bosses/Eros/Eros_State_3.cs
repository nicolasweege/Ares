using UnityEngine;

public class Eros_State_3 : Eros_State {
    private float _speed = 30f;
    private bool _canAttack = false;
    private Vector3 _attackDir;

    public override void EnterState(Eros context) {
        // teleport to outside of the screen
        FunctionTimer.Create(() => {
            var animPos = context.transform.position;
            context.transform.position = context.CenterMovePoint.position;
            Object.Instantiate(context.MainAnimation, animPos, Quaternion.identity);
        }, 0.3f, "Set Eros null position");

        // teleport to the right or left position
        FunctionTimer.Create(() => {
            Vector3 playerPos = Player.Instance.transform.position;
            if (playerPos.x < 0) {
                Object.Instantiate(context.MainAnimation, context.MovePointLeft.position, Quaternion.identity);
                context.transform.position = context.MovePointLeft.position;
            }

            if (playerPos.x >= 0) {
                Object.Instantiate(context.MainAnimation, context.MovePointRight.position, Quaternion.identity);
                context.transform.position = context.MovePointRight.position;
            }
        }, 0.5f, "Set Eros LEFT or RIGHT position");

        // aim the Player
        FunctionTimer.Create(() => {
            Vector3 playerPos = Player.Instance.transform.position;
            _attackDir = (playerPos - context.transform.position).normalized;
        }, 0.6f, "Get Player direction");

        // start attacking
        FunctionTimer.Create(() => _canAttack = true, 0.9f, "Set _canAttack to true");

        // stop attacking
        FunctionTimer.Create(() => _canAttack = false, 2f, "Set _canAttack to false");

        // teleport to outside of the screen
        FunctionTimer.Create(() => {
            var animPos = context.transform.position;
            context.transform.position = context.CenterMovePoint.position;
            Object.Instantiate(context.MainAnimation, animPos, Quaternion.identity);
        }, 2.1f, "Set Eros null position");

        // comeback to the center of the screen
        FunctionTimer.Create(() => {
            Object.Instantiate(context.MainAnimation, new Vector3(0, 0, 0), Quaternion.identity);
            context.transform.position = new Vector3(0, 0, 0);
        }, 2.2f, "Set Eros center position");

        // switch state
        FunctionTimer.Create(() => context.SwitchState(context.FirstState), 2.5f, "Switch state");
    }

    public override void UpdateState(Eros context) {
        if (Player.Instance == null || context == null)
            return;

        if (_canAttack) {
            if (context.Health <= 30)
                context.transform.position += _attackDir * Time.deltaTime * (_speed * 1.2f);
            else
                context.transform.position += _attackDir * Time.deltaTime * _speed;
        }
    }
}