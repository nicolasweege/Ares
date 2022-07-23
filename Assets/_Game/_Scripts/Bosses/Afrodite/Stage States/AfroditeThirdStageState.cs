using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeThirdStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 8f;
    private float _switchStateTimer;
    private float _timeToFirstWaveShoot = 0.2f;
    private float _firstWaveShootTimer;
    private float _timeToSecondWaveShoot = 0.2f;
    private float _secondWaveShootTimer;
    private float _timeToBreak = 3f;
    private float _breakTimer;
    private float _timeToShakeScreen = 0.5f;
    private float _screenShakeTimer;
    private bool _isFirstWaveFinished = false;
    private float _speedToMovePointCenter = 1f;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _breakTimer = _timeToBreak;
    }

    public override void UpdateState(AfroditeController context)
    {
        if (Vector2.Distance(context.transform.position, context.MovePointCenter.position) > 0.5f)
        {
            context.transform.position = Vector2.SmoothDamp(context.transform.position, context.MovePointCenter.position, ref context.Velocity, _speedToMovePointCenter);
        }
        else
        {
            _breakTimer -= Time.deltaTime;
            if (_breakTimer <= 0f)
            {
                _switchStateTimer -= Time.deltaTime;
                if (_switchStateTimer <= 0f)
                {
                    context.SwitchState(context.IdleState);
                }

                HandleAttack(context);
            }
            else
            {
                _screenShakeTimer -= Time.deltaTime;
                if (_screenShakeTimer <= 0f)
                {
                    CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
                    _screenShakeTimer = _timeToShakeScreen;
                }
            }
        }
    }

    private void HandleAttack(AfroditeController context)
    {
        if (!_isFirstWaveFinished)
        {
            _firstWaveShootTimer -= Time.deltaTime;
            if (_firstWaveShootTimer <= 0f)
            {
                for (int i = 0; i < context.ThirdStageFirstWaveShootDirections.Count; i++)
                {
                    GenerateBullet(context.transform, context.ThirdStageProjectile, context.ThirdStageFirstWaveShootDirections[i]);
                }
                _isFirstWaveFinished = true;
                _firstWaveShootTimer = _timeToFirstWaveShoot;
            }
        }

        if (_isFirstWaveFinished)
        {
            _secondWaveShootTimer -= Time.deltaTime;
            if (_secondWaveShootTimer <= 0f)
            {
                for (int i = 0; i < context.ThirdStageSecondWaveShootDirections.Count; i++)
                {
                    GenerateBullet(context.transform, context.ThirdStageProjectile, context.ThirdStageSecondWaveShootDirections[i]);
                }
                _isFirstWaveFinished = false;
                _secondWaveShootTimer = _timeToSecondWaveShoot;
            }
        }
    }

    private void GenerateBullet(Transform bulletStartingPos, GameObject bulletPrefab, Transform projectileDir)
    {
        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        Vector2 bulletDir = projectileDir.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }
}