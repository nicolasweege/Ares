using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFirstStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 5f;
    private float _switchStateTimer;
    private int _randomIndex;
    private Vector2 _currentMovePoint;
    private float _timeToFirstWaveShoot = 0.1f;
    private float _firstWaveShootTimer;
    private float _timeToSecondWaveShoot = 0.25f;
    private float _secondWaveShootTimer;
    private bool _isFirstWaveFinished = false;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _randomIndex = Random.Range(0, context.MovePoints.Count);
        _currentMovePoint = context.MovePoints[_randomIndex].position;
    }

    public override void UpdateState(AfroditeController context)
    {
        if (Vector2.Distance(context.transform.position, _currentMovePoint) < 0.5f)
        {
            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f)
            {
                context.SwitchState(context.FirstStageState);
            }
        }

        HandleMovement(context);
        HandleTurretAim(context.TurretTransform1);
        HandleTurretAim(context.TurretTransform2);
        HandleAttack(context);
    }

    private void HandleMovement(AfroditeController context)
    {
        Vector2 playerPos = PlayerMainShipController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), context.TurnSpeed * Time.deltaTime);
        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, context.Speed);
    }

    private void HandleAttack(AfroditeController context)
    {
        if (!_isFirstWaveFinished)
        {
            _firstWaveShootTimer -= Time.deltaTime;
            if (_firstWaveShootTimer <= 0f)
            {
                GenerateBullet(context, context.FirstStageProjectileStartingPoint1, context.FirstStageProjectile, context.FirstStageProjectileDir1);
                SoundManager.PlaySound(SoundManager.Sound.AfroditeFirstStageShoot, context.transform.position, 0.3f);
                _isFirstWaveFinished = true;
                _firstWaveShootTimer = _timeToFirstWaveShoot;
            }
        }

        if (_isFirstWaveFinished)
        {
            _secondWaveShootTimer -= Time.deltaTime;
            if (_secondWaveShootTimer <= 0f)
            {
                GenerateBullet(context, context.FirstStageProjectileStartingPoint2, context.FirstStageProjectile, context.FirstStageProjectileDir2);
                SoundManager.PlaySound(SoundManager.Sound.AfroditeFirstStageShoot, context.transform.position, 0.3f);
                _isFirstWaveFinished = false;
                _secondWaveShootTimer = _timeToSecondWaveShoot;
            }
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

    private Vector2 HandleTurretAim(Transform turretTransform)
    {
        Vector2 playerPos = PlayerMainShipController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(turretTransform.position.x, turretTransform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, Quaternion.Euler(0, 0, lookAngle), 20f * Time.deltaTime);
        return lookDir;
    }
}