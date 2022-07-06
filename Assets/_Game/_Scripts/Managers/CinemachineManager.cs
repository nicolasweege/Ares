using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : Singleton<CinemachineManager>
{
    private CinemachineVirtualCamera _cinemachineVMCam;

    protected override void Awake()
    {
        base.Awake();
        _cinemachineVMCam = GetComponent<CinemachineVirtualCamera>();
    }
}