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
        inputVector = target.position - transform.position;
        
        // Call the update method in the Creature class.
        // Done after input is retrieved.
        base.Update();
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Player")
        {
            hp--;
        }
    }
}
