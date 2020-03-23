using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertArrow : MonoBehaviour
{
    [HideInInspector] public Transform target;


    void Update()
    {
        // transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        transform.LookAt(target);
        // Vector3 rotation = bullet.transform.rotation.eulerAngles;
        // bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
    }
}
