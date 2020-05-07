using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertArrow : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public AlertArrowController.AlertReason reason;

    [HideInInspector] public AlertArrowController controllerRef;


    void Start()
    {
        switch (reason)
        {
            case AlertArrowController.AlertReason.Warning:
                target.gameObject.GetComponent<Child>().ChildDropped += RemoveArrow;
                target.gameObject.GetComponent<Child>().ChildTaken += RemoveArrow;
                break;
            case AlertArrowController.AlertReason.Direction:
                Player.PartDropped += RemoveArrow;
                break;
        }
        // if (reason == AlertArrowController.AlertReason.Warning)
        // {
        //     target.gameObject.GetComponent<Child>().ChildDropped += RemoveArrow;
        //     target.gameObject.GetComponent<Child>().ChildTaken += RemoveArrow;
        // }
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
        if (controllerRef)
        {
            controllerRef.arrows.Remove(this);
            controllerRef.NumOfArrows -= 1;
        }

        Destroy(gameObject);
    }


    void OnDisable()
    {
        // Disconnect delegates
        switch (reason)
        {
            case AlertArrowController.AlertReason.Warning:
                target.gameObject.GetComponent<Child>().ChildDropped -= RemoveArrow;
                target.gameObject.GetComponent<Child>().ChildTaken -= RemoveArrow;
                break;
            case AlertArrowController.AlertReason.Direction:
                Player.PartDropped -= RemoveArrow;
                break;
        }
    }
}
