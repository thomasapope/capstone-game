using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : Interactable
{
    public GameObject targetRef;

    public static event Action<Transform> ChildPickedUp = delegate {};
    public static event Action<Transform> ChildTaken = delegate {};


    public override void OnPickUp()
    {
        base.OnPickUp();

        // Check if being picked up by an enemy
        if (transform.parent.parent.gameObject.CompareTag("Enemy"))
        {
            ChildPickedUp(transform);
        }

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
