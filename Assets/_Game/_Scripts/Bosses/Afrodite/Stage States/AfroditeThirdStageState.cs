using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeThirdStageState : AfroditeBaseState
{
    private Vector2 _velocity = Vector2.zero;
    private float _timeToSwitchState = 8f;
    private float _switchStateTimer;
    private float _timeToShoot = 0.1f;
    private float _shootTimer;
    private float _timeToBreak = 3f;
    private float _breakTimer;
    private int _randomIndex;
    private Vector2 _currentBulletDir;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _breakTimer = _timeToBreak;
    }

    public override void UpdateState(AfroditeController context)
    {
        if (Vector2.Distance(context.transform.position, context.MovePointCenter.position) > 0.2f)
        {
            context.transform.position = Vector2.SmoothDamp(context.transform.position, context.MovePointCenter.position, ref _velocity, context.Speed);
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

                Attack(context);
            }

            CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
        }
    }

    private void Attack(AfroditeController context)
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            _randomIndex = Random.Range(0, context.ThirdStageProjectileDirections.Count);
            _currentBulletDir = context.ThirdStageProjectileDirections[_randomIndex].position;
            GenerateBullet(context, context.transform, context.ThirdStageProjectile);
            _shootTimer = _timeToShoot;
        }
    }

    private void GenerateBullet(AfroditeController context, Transform bulletStartingPos, GameObject bulletPrefab)
    {
        if (PlayerMainShipController.Instance == null)
            return;

        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        _currentBulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(_currentBulletDir.y, _currentBulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(_currentBulletDir.x, _currentBulletDir.y);
    }
}