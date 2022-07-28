using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;
    private Vector2 targetPosition = Vector2.zero;
    private Rect screenRect = Rect.zero;
    [Space(8)]
    [SerializeField] private float movementOffset = 10f;
    [SerializeField] float offsetMoveSpeed = 25f;
    private Vector3 cameraOffset = Vector2.zero;
    private float targetOffsetX = 0f;
    private Transform playerTransform = null;

    private void Start()
    {
        playerTransform = PlayerMainShipController.Instance.transform;
    }

    private void Update()
    {
        screenRect = new Rect(0f, 0f, Screen.width, Screen.height);

        targetOffsetX = PlayerMainShipController.Instance.MoveVector.x != 0f || PlayerMainShipController.Instance.MoveVector.y != 0f ?
            (PlayerMainShipController.Instance.MoveVector.x > 0f ? movementOffset : -movementOffset) : 0f;
        cameraOffset.x = Mathf.MoveTowards(cameraOffset.x, targetOffsetX, offsetMoveSpeed * Time.fixedDeltaTime);

        if (screenRect.Contains(Input.mousePosition))
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + cameraOffset;

        transform.position = Vector2.Lerp(playerTransform.position, targetPosition, sensitivity);
    }
}