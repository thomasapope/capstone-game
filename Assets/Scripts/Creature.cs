using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Creature.cs

    The superclass of all creatures and players. This is not meant to be used
    as is, but to be inherited from.
*/

public abstract class Creature : MonoBehaviour
{
    public Rigidbody rb;

    protected Vector3 inputVector;

    public float speed = 10f;


    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        Initialize();
    }
    
    
    protected virtual void Update()
    // Runs every frame. Virtual so that subclasses can override it.
    {
        Move();
    }


    void Move() 
    {
        // Logic necessary for moving the creature.
        // Uses the inputVector given it by the subclass and moves based on it.
        // This allows us to create movement behavior once and reuse it for 
        // most or all players and enemies.

        Vector3 velocity = new Vector3 (inputVector.x, rb.velocity.y, inputVector.z);
        velocity = velocity.normalized * speed;
       
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));

        rb.velocity = velocity;
    }

    // Initialize method must be overriden to update variables such as speed and health.
    public abstract void Initialize();
}
