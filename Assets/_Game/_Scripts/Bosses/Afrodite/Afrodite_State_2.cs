using UnityEngine;

public class Afrodite_State_2 : Afrodite_State {
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

    public override void EnterState(Afrodite context) {
        _switchStateTimer = _timeToSwitchState;
        _laserShootTimer = _timeToLaserShoot;
        _currentTurnSpeed = _initialTurnSpeed;
        _laserShootSoundPlayed = false;
        _laserLockOnSoundPlayed = false;

        if (Player.Instance.transform.position.x > 0) {
            _currentMovePoint = context.FourthStageMovePointRight.position;
        } else {
            _currentMovePoint = context.FourthStageMovePointLeft.position;
        }

        // if (context.transform.position.x > 0f) {
        //     _currentMovePoint = context.FourthStageMovePointRight.position;
        // } else {
        //     _currentMovePoint = context.FourthStageMovePointLeft.position;
        // }
    }

    public override void UpdateState(Afrodite context) {
        if (Vector2.Distance(context.transform.position, _currentMovePoint) > 0.5f) {
            HandleMovement(context);
            _currentTurnSpeed = _initialTurnSpeed;
            HandleAim(context);
        } else {
            HandleLaserShoot(context);
        }
    }

    private void HandleLaserShoot(Afrodite context) {
        _laserShootTimer -= Time.deltaTime;
        if (_laserShootTimer <= 0f) {
            context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().DisableFeedbackLaser();
            context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().EnableLaser();
            context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().UpdateLaser();

            if (!_laserShootSoundPlayed) {
                SoundManager.PlaySound(SoundManager.Sound.AfroditeSecondStageLaserShoot, context.transform.position, 0.3f);
                CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
                _laserShootSoundPlayed = true;
            }

            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f) {
                context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().DisableLaser();
                context.SwitchState(context.FirstState);
            }
        } else {
            if (_laserShootTimer <= 0.6f) {
                _currentTurnSpeed = _baseTurnSpeed;

                if (_laserShootTimer <= 0.4f) {
                    if (_laserShootTimer <= 0.1f) {
                        _currentTurnSpeed = _maxTurnSpeed;
                    }

                    if (!_laserLockOnSoundPlayed) {
                        SoundManager.PlaySound(SoundManager.Sound.AfroditeLaserLockOn, context.transform.position, 0.5f);
                        _laserLockOnSoundPlayed = true;
                    }
                    context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().EnableFeedbackLaser();
                    context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().UpdateFeedbackLaser();
                }
            } else {
                _currentTurnSpeed += 0.07f;
            }

            HandleAim(context);
        }
    }

    private void HandleMovement(Afrodite context) {
        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, _speedToMovePoint);
    }

    private void HandleAim(Afrodite context) {
        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), _currentTurnSpeed * Time.deltaTime);
    }
}