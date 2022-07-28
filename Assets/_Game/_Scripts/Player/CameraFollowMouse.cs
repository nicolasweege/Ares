using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    [SerializeField] private float threshold;

    private void Update()
    {
        Vector3 mousePos = Utils.GetMouseWorldPosition();
        Vector3 playerPos = PlayerMainShipController.Instance.transform.position;
        Vector3 targetPos = (playerPos + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + playerPos.x, threshold + playerPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + playerPos.y, threshold + playerPos.y);

        transform.position = targetPos;
    }
}