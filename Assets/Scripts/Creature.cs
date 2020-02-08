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
    // Component References
    private CharacterController controller;
    private LayerMask groundMask;
    public Transform groundCheck;

    protected Vector3 inputVector;
    private Vector3 velocity; // Used for gravity

    // Movement Stats
    public float speed = 10f;
    public bool usesGravity = true;
    public float gravity = -9.8f;
    public float groundDistance = 0.4f;

    private bool isGrounded;

    // Combat Stats
    public float hp = 1;


    protected virtual void Start()
    {
        controller = this.GetComponent<CharacterController>(); // Get the CharacterController at runtime
        groundMask = LayerMask.NameToLayer("Ground"); // Get the layermask for the ground

        // Run Initialize method as defined in subclass
        Initialize();
    }
    
    
    protected virtual void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Move();
        CheckIfDead();
    }


    private void CheckIfDead()
    {
        if (hp <= 0)
        {
            OnDeath();
        }
    }

    // Logic necessary for moving the creature.
    // Uses the inputVector given it by the subclass and moves based on it.
    // This allows us to create movement behavior once and reuse it for 
    // most or all players and enemies.
    void Move() 
    {
        Vector3 move = (transform.right * inputVector.x + transform.forward * inputVector.z).normalized;

        controller.Move(move * speed * Time.deltaTime);

        if (usesGravity) 
        {
            Gravity();
        }
    }


    // Adds a downward force to the creature
    void Gravity()
    {

        // Make sure gravity doesn't increase infinitely.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    protected virtual void OnDeath()
    {
        // What happens when something dies
        Destroy(gameObject);
    }

    // Initialize method must be overriden to update variables such as speed and health.
    public abstract void Initialize();
}
