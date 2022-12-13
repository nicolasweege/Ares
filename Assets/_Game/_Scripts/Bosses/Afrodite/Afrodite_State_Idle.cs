using UnityEngine;

public class Afrodite_State_Idle : Afrodite_State {
    private float _timeToSwitchState = 2f;
    private float _timer;

    public override void EnterState(Afrodite context) {
        _timer = _timeToSwitchState;
    }

    public override void UpdateState(Afrodite context) {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            context.SwitchState(context.FirstState);
    }
}