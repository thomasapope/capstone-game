using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Player.cs

    A class meant to hold the behavioral code for player characters.
*/

public class Player : Creature
{
    public override void Initialize()
    {
        movementSpeed = 10f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Update the inputVector for movement
        // inputVector.x = Input.GetAxisRaw("Horizontal");
        // inputVector.z = Input.GetAxisRaw("Vertical");
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.z = Input.GetAxisRaw("Vertical");

        // Call the update method in the Creature class.
        // Done after input is retrieved.
        base.Update();
    }
}
