using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : Interactable
{
    public GameObject targetRef;

    public event Action<Transform> ChildPickedUp = delegate {};
    public event Action<Transform> ChildTaken = delegate {};


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

    void OnDisable()
    {
        if (GameManager.targetRefs.Contains(gameObject))
        {
            GameManager.targetRefs.Remove(gameObject);
        }

        ChildTaken(transform);
    }
}
