using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeDeathState : AfroditeBaseState
{
    private float _timeToDeathAnim = 2f;
    private float _deathAnimTimer;
    private float _timeToShakeScreen = 0.5f;
    private float _screenShakeTimer;

    public override void EnterState(AfroditeController context)
    {
        _deathAnimTimer = _timeToDeathAnim;
    }

    public override void UpdateState(AfroditeController context)
    {
        HandleDeath(context);
    }

    private void HandleDeath(AfroditeController context)
    {
        _deathAnimTimer -= Time.deltaTime;
        if (_deathAnimTimer <= 0f)
        {
            Object.Destroy(context.gameObject);
            Object.Instantiate(context.DeathAnim, context.transform.position, Quaternion.identity);
            context.SwitchState(context.IdleState);
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