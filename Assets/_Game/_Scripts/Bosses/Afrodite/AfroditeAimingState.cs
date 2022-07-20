using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeAimingState : AfroditeBaseState
{
    private float _timeToSwitchState = 5f;
    private float _timer;
    private Vector2 _velocity = Vector2.zero;
    private int _randomIndex;
    private Vector2 _currentMovePoint;
    private float _timeToFirstStage = 1f;
    private float _firstStageTimer;

    public override void EnterState(AfroditeController context)
    {
        _timer = _timeToSwitchState;
        _randomIndex = Random.Range(0, context.MovePoints.Count);
        _currentMovePoint = context.MovePoints[_randomIndex].position;
    }

    public override void UpdateState(AfroditeController context)
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            context.SwitchState(context.AimingState);
        }

        Vector2 playerPos = PlayerMainShipController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), context.TurnSpeed * Time.deltaTime);
        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref _velocity, context.Speed);

        HandleFirstStage(context);
    }

    private void HandleFirstStage(AfroditeController context)
    {
        bool isEnemyVisible = context.GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        _firstStageTimer -= Time.deltaTime;
        if (_firstStageTimer <= 0f)
        {
            context.GenerateBullet(context, context.FirstStageBulletStartingPoint, context.FirstStageBullet);
            _firstStageTimer = _timeToFirstStage;
        }
    }
}