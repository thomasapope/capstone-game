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
        movementSpeed = 4f;
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


    private void OnDisable()
    {
        GameStats.score++; // Add one kill to the score
    }
    // protected override void OnDeath() 
    // {
    //     GameStats.score++;
    //     Debug.Log("Score!");
    //     base.OnDeath();
    // }
}
