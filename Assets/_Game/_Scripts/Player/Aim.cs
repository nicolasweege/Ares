using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    private PlayerController _playerScript;
    private void Start() => _playerScript = GetComponent<PlayerController>();

    private void Update() => PlayerAim();

    public Vector2 PlayerAim()
    {
        Vector2 mousePos;
        Vector2 lookDir;
        float lookAngle;

        mousePos = _playerScript.Cam.ScreenToWorldPoint(Input.mousePosition);
        lookDir = mousePos - _playerScript.Rb.position;
        lookDir.Normalize();

        lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        return lookDir;
    }
}