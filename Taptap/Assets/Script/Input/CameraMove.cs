using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float MoveSpeed;
    public float ZoomSpeed = 10f; // 缩放速度
    public float MinZoom = 5f; // 最小缩放值
    public float MaxZoom = 20f; // 最大缩放值
    public Camera mainCamera;

    public Transform leftUp;
    public Transform rightDown;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();

    }
    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            CameraMoveTo(Vector2.up, Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            CameraMoveTo(Vector2.down, Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            CameraMoveTo(Vector2.left, Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            CameraMoveTo(Vector2.right, Time.deltaTime);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            float newSize = mainCamera.orthographicSize - scroll * ZoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(newSize, MinZoom, MaxZoom);
        }
    }

    private void CameraMoveTo(Vector2 dir,float delta)
    {
        Vector3 pos = mainCamera.transform.transform.position + new Vector3(dir.x, dir.y, 0) * MoveSpeed * delta;
        pos.x = Mathf.Clamp(pos.x, leftUp.position.x, rightDown.position.x);
        pos.y = Mathf.Clamp(pos.y, rightDown.position.y, leftUp.position.y);
        mainCamera.transform.position = pos;
    }
}
