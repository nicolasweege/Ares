using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        Vector2 mousePosition = GetMousePosition();
        transform.position = new Vector3(mousePosition.x, mousePosition.y);
    }

    private Vector2 GetMousePosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}