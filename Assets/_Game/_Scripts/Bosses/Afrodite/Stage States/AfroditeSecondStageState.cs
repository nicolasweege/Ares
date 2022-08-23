using UnityEngine;

public class AfroditeSecondStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 2f;
    private float _switchStateTimer;
    private float _initialTurnSpeed = 1f;
    private float _baseTurnSpeed = 15f;
    private float _maxTurnSpeed = 20f;
    private float _currentTurnSpeed;
    private float _timeToLaserShoot = 2f;
    private float _laserShootTimer;
    private bool _laserShootSoundPlayed = false;
    private Vector2 _currentMovePoint;
    private float _speedToMovePoint = 1.5f;
    private bool _laserLockOnSoundPlayed = false;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _laserShootTimer = _timeToLaserShoot;
        _currentTurnSpeed = _initialTurnSpeed;
        _laserShootSoundPlayed = false;
        _laserLockOnSoundPlayed = false;

        if (context.transform.position.x > 0f) {
            _currentMovePoint = context.FourthStageMovePointLeft.position;
        }
        else {
            _currentMovePoint = context.FourthStageMovePointRight.position;
        }
    }

    public override void UpdateState(AfroditeController context)
    {
        if (Vector2.Distance(context.transform.position, _currentMovePoint) > 0.5f)
        {
            HandleMovement(context);
            _currentTurnSpeed = _initialTurnSpeed;
            HandleAim(context);
        } else {
            HandleLaserShoot(context);
        }
    }

    private void HandleLaserShoot(AfroditeController context)
    {
        _laserShootTimer -= Time.deltaTime;
        if (_laserShootTimer <= 0f)
        {
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableFeedbackLaser();
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableLaser();
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateLaser();

            if (!_laserShootSoundPlayed)
            {
                SoundManager.PlaySound(SoundManager.Sound.AfroditeSecondStageLaserShoot, context.transform.position, 0.3f);
                CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
                _laserShootSoundPlayed = true;
            }

            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f)
            {
                context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
                context.SwitchState(context.FirstStageState);
            }
        }
        else
        {
            if (_laserShootTimer <= 0.6f)
            {
                _currentTurnSpeed = _baseTurnSpeed;

                if (_laserShootTimer <= 0.4f)
                {
                    if (_laserShootTimer <= 0.1f)
                    {
                        _currentTurnSpeed = _maxTurnSpeed;
                    }

                    if (!_laserLockOnSoundPlayed)
                    {
                        SoundManager.PlaySound(SoundManager.Sound.AfroditeLaserLockOn, context.transform.position, 0.3f);
                        _laserLockOnSoundPlayed = true;
                    }
                    context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableFeedbackLaser();
                    context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateFeedbackLaser();
                }
            } else {
                _currentTurnSpeed += 0.07f;
            }

            HandleAim(context);
        }
    }

    private void HandleMovement(AfroditeController context)
    {
        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, _speedToMovePoint);
    }

    private void HandleAim(AfroditeController context)
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), _currentTurnSpeed * Time.deltaTime);
    }
}