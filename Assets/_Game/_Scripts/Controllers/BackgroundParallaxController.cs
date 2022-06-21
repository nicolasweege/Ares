using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallaxController : MonoBehaviour
{
    [SerializeField] private bool _stars;
    [SerializeField] private Vector2 _parallaxMultiplier;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPos;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPos = _cameraTransform.position;
    }

    private void LateUpdate()
    {
        if (_stars)
            HandleStarsParallax();
        if (!_stars)
            HandleNormalParallax();
    }

    private void HandleNormalParallax()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPos;
        transform.position += new Vector3(deltaMovement.x * _parallaxMultiplier.x, deltaMovement.y * _parallaxMultiplier.y, 0f);
        _lastCameraPos = _cameraTransform.position;
    }

    private void HandleStarsParallax()
    {

    }
}