using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDamageController : MonoBehaviour
{
    [SerializeField] private float _timeToAutoDestroy = 0.2f;
    private float _autoDestroyTimer;

    private void Awake() => _autoDestroyTimer = _timeToAutoDestroy;

    private void Update()
    {
        _autoDestroyTimer -= Time.deltaTime;
        if (_autoDestroyTimer <= 0f)
            Destroy(gameObject);
    }
}