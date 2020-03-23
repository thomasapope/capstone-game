using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 200f;
    private float xRotation = 0f;
    public bool fps = false;
    public Transform container;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

	void LateUpdate()
	{
        if(Input.GetKeyDown(KeyCode.M))
        {
            fps = fps == false ? fps = true : false;
        }
        if (fps == true)
        {
            container.localPosition = new Vector3(0, 1.5f, 1);
        } else {
            container.localPosition = new Vector3(0, 2, -5);
        }
        

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}