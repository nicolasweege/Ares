using Cinemachine;
using UnityEngine.Events;

public class CinemachineManager : Singleton<CinemachineManager>
{
    private CinemachineVirtualCamera _cinemachineVMCam;

    protected override void Awake()
    {
        base.Awake();
        _cinemachineVMCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ScreenShakeEvent(UnityEvent screenShakeEvent) => screenShakeEvent.Invoke();
}