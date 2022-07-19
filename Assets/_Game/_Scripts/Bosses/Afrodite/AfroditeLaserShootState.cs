using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeLaserShootState : AfroditeBaseState
{
    private float _timeToSwitchState = 5f;
    private float _timer;

    public override void EnterState(AfroditeController context)
    {
        context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableLaser();
        _timer = _timeToSwitchState;
    }

    public override void UpdateState(AfroditeController context)
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
            context.SwitchState(context.AimingState);
        }

        context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateLaser();
    }
}