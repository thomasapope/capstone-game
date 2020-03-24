using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertArrow : MonoBehaviour
{
    [HideInInspector] public Transform target;


    void Start()
    {
        target.gameObject.GetComponent<Child>().ChildDropped += RemoveArrow;
        target.gameObject.GetComponent<Child>().ChildTaken += RemoveArrow;
    }


    void Update()
    {
        if (!target)
        {
            RemoveArrow();
        }

        transform.LookAt(target);
    }


    void RemoveArrow()
    {
        Destroy(gameObject);
    }


    void OnDisable()
    {
        target.gameObject.GetComponent<Child>().ChildDropped -= RemoveArrow;
        target.gameObject.GetComponent<Child>().ChildTaken -= RemoveArrow;
    }
}
