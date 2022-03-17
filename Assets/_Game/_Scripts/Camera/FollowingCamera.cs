using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    private void Update() {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        float xx = Mathf.Lerp(transform.position.x, _playerTransform.position.x, 0.01f);
        float yy = Mathf.Lerp(transform.position.y, _playerTransform.position.y, 0.01f);
        transform.position = new Vector3(xx, yy, transform.position.z);
    }
}