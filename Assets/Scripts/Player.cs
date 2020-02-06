﻿using System.Collections;
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
        speed = 10f;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        // Update the inputVector for movement
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");

        // Call the update method in the Creature class.
        // Done after input is retrieved.
        base.FixedUpdate();
    }
}
