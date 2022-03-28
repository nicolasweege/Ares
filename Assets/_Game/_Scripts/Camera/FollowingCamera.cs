using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    private Transform _playerTransform;

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        float xx = Mathf.Lerp(transform.position.x, _playerTransform.position.x, 1.4f * Time.deltaTime);
        float yy = Mathf.Lerp(transform.position.y, _playerTransform.position.y, 1.4f * Time.deltaTime);
        transform.position = new Vector3(xx, yy, transform.position.z);
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this._playerTransform = playerTransform;
    }
}