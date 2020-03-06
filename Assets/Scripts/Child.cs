using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : Interactable
{
    public GameObject targetRef;

    void Start()
    {
        // targetRef = gameObject.GetComponentInChildren<Transform>().gameObject;
        // targetRef = transform.GetChild(1).gameObject;
        // Debug.Log(targetRef.name);
    }

    public override void PickUpObject()
    {
        base.PickUpObject();

        targetRef.SetActive(false);
    }
}
