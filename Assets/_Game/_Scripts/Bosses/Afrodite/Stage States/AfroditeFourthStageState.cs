using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 7f;
    private float _switchStateTimer;
    private Vector2 _currentMovePoint;
    private Vector2 _currentAimPoint;
    private Vector2 _otherAimPoint;
    private float _baseTurnSpeed = 0.4f;
    private float _currentTurnSpeed;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;

        if (PlayerMainShipController.Instance.transform.position.x > 0f)
        {
            _currentMovePoint = context.FourthStageMovePointLeft.position;
        }
        else
        {
            _currentMovePoint = context.FourthStageMovePointRight.position;
        }

        if (PlayerMainShipController.Instance.transform.position.y > 0f)
        {
            _currentAimPoint = context.FourthStageAimPointDown.position;
            _otherAimPoint = context.FourthStageAimPointUp.position;
        }
        else
        {
            _currentAimPoint = context.FourthStageAimPointUp.position;
            _otherAimPoint = context.FourthStageAimPointDown.position;
        }
    }

    public override void UpdateState(AfroditeController context)
    {
        
        if (Vector2.Distance(context.transform.position, _currentMovePoint) > 0.5f)
        {
            _currentTurnSpeed = 1f;
            HandleAim(context, _otherAimPoint);
            HandleMovement(context);
        }
        else
        {
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableLaser();
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateLaser();
            _currentTurnSpeed = _baseTurnSpeed;
            HandleAim(context, _currentAimPoint);

            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f)
            {
                context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
                context.SwitchState(context.IdleState);
            }
        }
    }

    private void HandleMovement(AfroditeController context)
    {
        if (PlayerMainShipController.Instance == null)
            return;

        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, context.Speed);
    }

    private void HandleAim(AfroditeController context, Vector2 aimPoint)
    {
        Vector2 lookDir = aimPoint - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), _currentTurnSpeed * Time.deltaTime);
    }
}