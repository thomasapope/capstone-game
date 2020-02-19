using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Script that causes the camera to follow the player.
*/

public class CameraController : MonoBehaviour
{
    // public Transform target;
    
    // public float smoothSpeed = 0.125f;
    // public Vector3 offset;
    // private Vector3 velocity = Vector3.zero;

    // void LateUpdate() 
    // {
    //     Vector3 desiredPosition = target.position + offset;
    //     //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    //     Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    //     transform.position = smoothedPosition;
    // }
    public float rotationSpeed = 1;
    private Vector2 mouse;
    float mouseX, mouseY;

    public Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float pitch = 2f;

    private float currentZoom = 10f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        // Zooming
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }


    void LateUpdate()
    {
        CameraControl();
    }


    void CameraControl()
    {
        // mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        // mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouse.x += Input.GetAxis("Mouse X") * rotationSpeed;
        mouse.y -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouse.y = Mathf.Clamp(mouse.y, -35, 60);

        transform.LookAt(target);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            target.rotation = Quaternion.Euler(mouse.y, mouse.x, 0);
        }
        else
        {
            transform.position = target.position - offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch);
        }
    }
}
