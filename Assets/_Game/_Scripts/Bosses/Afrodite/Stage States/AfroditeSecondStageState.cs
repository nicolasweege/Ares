using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeSecondStageState : AfroditeBaseState
{
    private float _timeToSwitchState = 2f;
    private float _switchStateTimer;
    private float _initialTurnSpeed = 1f;
    private float _baseTurnSpeed = 15f;
    private float _maxTurnSpeed = 20f;
    private float _currentTurnSpeed;
    private float _timeToLaserShoot = 2.3f;
    private float _laserShootTimer;
    private bool _laserShootAudioHasPlayed = false;

    public override void EnterState(AfroditeController context)
    {
        _switchStateTimer = _timeToSwitchState;
        _laserShootTimer = _timeToLaserShoot;
        _currentTurnSpeed = _initialTurnSpeed;
        _laserShootAudioHasPlayed = false;
    }

    public override void UpdateState(AfroditeController context)
    {
        _laserShootTimer -= Time.deltaTime;
        if (_laserShootTimer <= 0f)
        {
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableFeedbackLaser();
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableLaser();
            context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateLaser();

            if (!_laserShootAudioHasPlayed)
            {
                SoundManager.PlaySound(SoundManager.Sound.AfroditeSecondStageLaserShoot, context.transform.position, 0.3f);
                CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
                _laserShootAudioHasPlayed = true;
            }

            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f)
            {
                context.LaserBeam.GetComponent<AfroditeLaserBeamController>().DisableLaser();
                context.SwitchState(context.IdleState);
            }
        }
        else
        {
            if (_laserShootTimer <= 0.6f)
            {
                _currentTurnSpeed = _baseTurnSpeed;

                if (_laserShootTimer <= 0.3f)
                {
                    if (_laserShootTimer <= 0.1f)
                        _currentTurnSpeed = _maxTurnSpeed;

                    context.LaserBeam.GetComponent<AfroditeLaserBeamController>().EnableFeedbackLaser();
                    context.LaserBeam.GetComponent<AfroditeLaserBeamController>().UpdateFeedbackLaser();
                }
            }
            else
            {
                _currentTurnSpeed += 0.05f;
            }

            HandleAim(context);
        }
    }

    private void HandleAim(AfroditeController context)
    {
        Vector2 playerPos = PlayerMainShipController.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), _currentTurnSpeed * Time.deltaTime);
    }
}