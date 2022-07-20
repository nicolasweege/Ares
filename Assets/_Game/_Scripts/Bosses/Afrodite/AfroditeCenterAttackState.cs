using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeCenterAttackState : AfroditeBaseState
{
    private Vector2 _velocity = Vector2.zero;
    private float _timeToSwitchState = 15f;
    private float _timer;

    public override void EnterState(AfroditeController context)
    {
        _timer = _timeToSwitchState;
    }

    public override void UpdateState(AfroditeController context)
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            context.SwitchState(context.IdleState);
        }

        context.transform.position = Vector2.SmoothDamp(context.transform.position, context.MovePointCenter.position, ref _velocity, context.Speed);
    }
}