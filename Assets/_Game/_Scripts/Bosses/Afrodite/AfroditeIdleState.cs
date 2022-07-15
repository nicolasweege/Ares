using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeIdleState : AfroditeBaseState
{
    private float _timeToSwitchState = 3f;
    private float _timer;
    private Vector2 _startingPos;

    public override void EnterState(AfroditeController context)
    {
        _timer = _timeToSwitchState;
        _startingPos.x = context.transform.position.x;
        _startingPos.y = context.transform.position.y;
    }

    public override void UpdateState(AfroditeController context)
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            context.SwitchState(context.AimingState);
        }

        var speed = 1.0f;
        var amount = 0.1f;
        context.transform.position = new Vector3(_startingPos.x + Mathf.Sin(Time.time * speed) * amount, _startingPos.y + (Mathf.Cos(Time.time * speed) * amount), context.transform.position.z);
    }
}