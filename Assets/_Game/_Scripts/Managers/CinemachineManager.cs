using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : Singleton<CinemachineManager>
{
    private CinemachineVirtualCamera _cinemachineVMCam;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    protected override void Awake()
    {
        base.Awake();
        _cinemachineVMCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    _cinemachineVMCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, _shakeTimer / _shakeTimerTotal);
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            _cinemachineVMCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _startingIntensity = intensity;
        _shakeTimerTotal = time;
        _shakeTimer = time;
    }
}