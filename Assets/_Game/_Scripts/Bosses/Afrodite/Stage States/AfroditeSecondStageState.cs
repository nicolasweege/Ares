using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeSecondStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 1.5f;
    private float _switchStateTimer;
    private int _randomIndex;
    private Vector2 _currentMovePoint;
    private float _speedToCurrentMovePoint = 1f;
    private float _turnSpeed = 5f;
    private float _timeToLaserShoot = 2f;
    private float _laserShootTimer;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _laserShootTimer = _timeToLaserShoot;
        _randomIndex = Random.Range(0, context.MovePoints.Count);
        _currentMovePoint = context.MovePoints[_randomIndex].position;
    }

    public override void UpdateState(AfroditeController context)
    {
        /*if (Vector2.Distance(context.transform.position, _currentMovePoint) > 0.5f)
        {
            Vector2 playerPos = PlayerMainShipController.Instance.transform.position;
            Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
            lookDir.Normalize();
            float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), _turnSpeed * Time.deltaTime);
            // context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, _speedToCurrentMovePoint);
        }*/

        _laserShootTimer -= Time.deltaTime;
        if (_laserShootTimer <= 0f)
        {
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableLaser();
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateLaser();

            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f)
            {
                context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
                context.SwitchState(context.IdleState);
            }
        }
        else
        {
            Vector2 playerPos = PlayerMainShipController.Instance.transform.position;
            Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
            lookDir.Normalize();
            float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), _turnSpeed * Time.deltaTime);
        }
    }
}