using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFirstStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 5f;
    private float _switchStateTimer;
    private Vector2 _velocity = Vector2.zero;
    private int _randomIndex;
    private Vector2 _currentMovePoint;
    private float _timeToShoot = 1f;
    private float _shootTimer;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _randomIndex = Random.Range(0, context.MovePoints.Count);
        _currentMovePoint = context.MovePoints[_randomIndex].position;
    }

    public override void UpdateState(AfroditeController context)
    {
        _switchStateTimer -= Time.deltaTime;
        if (_switchStateTimer <= 0f)
        {
            context.SwitchState(context.FirstStageState);
        }

        Vector2 playerPos = PlayerMainShipController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), context.TurnSpeed * Time.deltaTime);
        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref _velocity, context.Speed);

        Attack(context);
    }

    private void Attack(AfroditeController context)
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            GenerateBullet(context, context.FirstStageProjectileStartingPoint, context.FirstStageProjectile, context.FirstStageProjectileDir);
            _shootTimer = _timeToShoot;
        }
    }

    private void GenerateBullet(AfroditeController context, Transform bulletStartingPos, GameObject bulletPrefab, Transform projectileDir)
    {
        if (PlayerMainShipController.Instance == null)
            return;

        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        context.CurrentFirstStageProjectileDir = projectileDir.position - bulletInst.transform.position;
        context.CurrentFirstStageProjectileDir.Normalize();
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(context.CurrentFirstStageProjectileDir.x, context.CurrentFirstStageProjectileDir.y);
    }
}