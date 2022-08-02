using UnityEngine;

public class AfroditeFourthStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 5f;
    private float _switchStateTimer;
    private Vector2 _currentMovePoint;
    private Vector2 _currentAimPoint;
    private Vector2 _otherAimPoint;
    private Vector2 _currentSatellitePoint;
    private float _baseTurnSpeed = 0.8f;
    private float _currentTurnSpeed;
    private float _speedToMovePoint = 1f;
    private GameObject _satelliteInst;
    private float _timeToCreateSatellite = 2f;
    private float _createSatelliteTimer;
    private bool _isSatelliteInstantiated = false;
    private bool _laserShootAudioHasPlayed = false;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _createSatelliteTimer = _timeToCreateSatellite;
        _isSatelliteInstantiated = false;
        _laserShootAudioHasPlayed = false;

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
            _currentAimPoint = context.FourthStageAimPointUp.position;
            _otherAimPoint = context.FourthStageAimPointDown.position;
            _currentSatellitePoint = context.FourthStageSatelliteUpPoint.position;
        }
        else
        {
            _currentAimPoint = context.FourthStageAimPointDown.position;
            _otherAimPoint = context.FourthStageAimPointUp.position;
            _currentSatellitePoint = context.FourthStageSatelliteDownPoint.position;
        }
    }

    public override void UpdateState(AfroditeController context)
    {
        if (Vector2.Distance(context.transform.position, _currentMovePoint) > 1f)
        {
            _currentTurnSpeed = 1f;
            HandleAim(context, _otherAimPoint);
            HandleMovement(context);

            _createSatelliteTimer -= Time.deltaTime;
            if (_createSatelliteTimer <= 0f)
            {
                if (!_isSatelliteInstantiated)
                {
                    _satelliteInst = Object.Instantiate(context.FourthStageSatellite, _currentSatellitePoint, context.transform.rotation);
                    _isSatelliteInstantiated = true;
                }
            }
        }
        else
        {
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableLaser();
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateLaser();
            if (!_laserShootAudioHasPlayed)
            {
                SoundManager.PlaySound(SoundManager.Sound.AfroditeSecondStageLaserShoot, context.transform.position, 0.3f);
                CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
                _laserShootAudioHasPlayed = true;
            }

            _currentTurnSpeed = _baseTurnSpeed;
            HandleAim(context, _currentAimPoint);

            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f)
            {
                context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
                _satelliteInst.GetComponent<AfroditeFourthStageSatelliteController>().HandleDeath();
                _isSatelliteInstantiated = false;
                context.SwitchState(context.FirstStageState);
            }
        }
    }

    private void HandleMovement(AfroditeController context)
    {
        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, _speedToMovePoint);
    }

    private void HandleAim(AfroditeController context, Vector2 aimPoint)
    {
        Vector2 lookDir = aimPoint - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), _currentTurnSpeed * Time.deltaTime);
    }
}