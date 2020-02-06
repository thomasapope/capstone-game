using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Enemy.cs

    The base class for simple enemies. Includes pathfinding behaviors.
*/


public class Enemy : Creature
{
    public Transform target;

    public override void Initialize()
    {
        speed = 4f;
    }

   // Update is called once per frame
    protected override void Update()
    {
        // Update the inputVector for movement
        // inputVector.x = Input.GetAxis("Horizontal");
        // inputVector.z = Input.GetAxis("Vertical");
        inputVector = target.position - transform.position;
        // Debug.Log(direction);

        // float angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        // // rb.rotation = angle;

        // inputVector = direction;

        // Call the update method in the Creature class.
        // Done after input is retrieved.
        base.Update();
    }
}
