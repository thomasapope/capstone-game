﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollide : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float zoomSpeed = 10.0f;
    Vector3 cameraDirection;
    public float distance;

    void Awake() {
        cameraDirection = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(cameraDirection * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast (transform.parent.position, desiredCameraPos, out hit)) {
            distance = Mathf.Clamp ((hit.distance * .7f), minDistance, maxDistance);

        } else {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp (transform.localPosition, cameraDirection * distance, Time.deltaTime * zoomSpeed);
        }
}
