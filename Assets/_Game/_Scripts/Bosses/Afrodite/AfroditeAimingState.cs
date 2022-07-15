using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeAimingState : AfroditeBaseState
{
    private float _timeToSwitchState = 3f;
    private float _timer;
    private Vector2 _velocity = Vector2.zero;

    public override void EnterState(AfroditeController context)
    {
        _timer = _timeToSwitchState;
    }

    public override void UpdateState(AfroditeController context)
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            context.SwitchState(context.LaserShootState);
        }

        Vector2 playerPos = PlayerSubAttackShipController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), context.TurnSpeed * Time.deltaTime);
        context.transform.position = Vector2.SmoothDamp(context.transform.position, new Vector2(5f, 0f), ref _velocity, context.Speed);
    }
}