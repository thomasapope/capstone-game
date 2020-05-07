using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 200f;
    private float xRotation = 0f;
    public Transform container;
    public bool CursorLocked = true;

    void Start()
    {
        Cursor.lockState = CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

	void LateUpdate()
	{
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        if (!GameManager.instance.gameHasEnded)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            container.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }
}