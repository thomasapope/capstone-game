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
    private CharacterController controller;
    public Transform groundCheck;

    protected Vector3 inputVector;
    Vector3 velocity;

    public float speed = 10f;
    public float gravity = -9.8f;
    public bool usesGravity = true;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;


    protected virtual void Start()
    {
        // Get the CharacterController at runtime
        controller = this.GetComponent<CharacterController>();

        Initialize();
    }
    
    
    protected virtual void Update()
    // For physics processes only.
    // Virtual so that subclasses can override it.
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Move();
    }


    void Move() 
    {
        // Logic necessary for moving the creature.
        // Uses the inputVector given it by the subclass and moves based on it.
        // This allows us to create movement behavior once and reuse it for 
        // most or all players and enemies.

        Vector3 move = (transform.right * inputVector.x + transform.forward * inputVector.z).normalized;

        controller.Move(move * speed * Time.deltaTime);

        if (usesGravity) 
        {
            Gravity();
        }
    }

    void Gravity()
    {
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Adds a downward force to the creature
        velocity.y += gravity * Time.deltaTime;


        controller.Move(velocity * Time.deltaTime);
    }

    // Initialize method must be overriden to update variables such as speed and health.
    public abstract void Initialize();
}
