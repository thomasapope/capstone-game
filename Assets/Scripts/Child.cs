﻿using System.Collections;
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

    public override void OnPickUp()
    {
        base.OnPickUp();

        targetRef.SetActive(false);
    }
    

    public override void OnDrop()
    {
        base.OnDrop();

        targetRef.SetActive(true);
    }
}
