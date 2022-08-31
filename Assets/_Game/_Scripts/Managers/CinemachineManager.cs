using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineManager : Singleton<CinemachineManager> {
    public float BaseOrthographicSize = 7f;
    [SerializeField] private Transform _followTransform;
    [NonSerialized] public CinemachineVirtualCamera CinemachineVirtualCamera;
    private bool _isZoomingIn = false;
    private bool _isZoomingOut = false;
    private float _targetZoomValue;
    private float _changeSizeAmount;

    protected override void Awake() {
        base.Awake();

        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);
        if (brain == null) {
            brain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }

        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera.Follow = _followTransform;
        CinemachineVirtualCamera.Priority = 1;
    }

    private void Update() {
        if (_isZoomingIn) {
            if (CinemachineVirtualCamera.m_Lens.OrthographicSize <= _targetZoomValue)
                _isZoomingIn = false;
            
            CinemachineVirtualCamera.m_Lens.OrthographicSize -= _changeSizeAmount * Time.deltaTime;
        }

        if (_isZoomingOut) {
            if (CinemachineVirtualCamera.m_Lens.OrthographicSize >= _targetZoomValue)
                _isZoomingOut = false;
            
            CinemachineVirtualCamera.m_Lens.OrthographicSize += _changeSizeAmount * Time.deltaTime;
        }
    }

    public void ScreenShakeEvent(UnityEvent screenShakeEvent) {
        screenShakeEvent.Invoke();
    }

    public void ZoomIn(float targetValue, float changeSizeAmount) {
        _targetZoomValue = targetValue;
        _changeSizeAmount = changeSizeAmount;
        _isZoomingIn = true;
    }

    public void ZoomOut(float targetValue, float changeSizeAmount) {
        _targetZoomValue = targetValue;
        _changeSizeAmount = changeSizeAmount;
        _isZoomingOut = true;
    }

    public void ResetOrthographicSize() {
        CinemachineVirtualCamera.m_Lens.OrthographicSize = BaseOrthographicSize;
    }

    public void SetTargetTransform(Transform transform) {
        CinemachineVirtualCamera.Follow = transform;
    }
}