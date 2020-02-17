using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public Vector3 velocity; // Used for gravity

    // Movement Stats
    public float speed = 10f;
    public bool usesGravity = true;
    public float gravity = -20f;
    public float groundDistance = 0.4f;

    private bool isGrounded;

    // Combat Stats
    // [SerializeField]
    // private int MAX_HEALTH = 100;

    public Health stats;

    // public int hp { get; private set; }


    // Initialize method must be overriden to update variables such as speed and health.
    public abstract void Initialize();


    protected virtual void Start()
    {
        controller = this.GetComponent<CharacterController>(); // Get the CharacterController at runtime
        groundMask = LayerMask.NameToLayer("Ground"); // Get the layermask for the ground
        //groundMask = 8;
        
        // hp = MAX_HEALTH;
        // OnHealthAdded(this);
        stats = gameObject.GetComponent<Health>();

        // Run Initialize method as defined in subclass
        Initialize();
    }
    
    
    protected virtual void Update()
    {
        RaycastHit hit;
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out hit, 1.4f);



        Move();
        // CheckIfDead();
    }


    // Logic necessary for moving the creature.
    // Uses the inputVector given it by the subclass and moves based on it.
    // This allows us to create movement behavior once and reuse it for 
    // most or all players and enemies.
    void Move() 
    {
        // Vector3 move = (transform.right * inputVector.x + transform.forward * inputVector.z).normalized;
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));

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


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "sword")
        {
            stats.ModifyHealth(-10);
            Debug.Log("Sword hit");
        }
        if (hit.gameObject.name == "EnemyAttack")
        {
            stats.ModifyHealth(-1);
            Debug.Log("Enemy hit");
        }
    }


    // private void OnDisable() 
    // {
    //     OnHealthRemoved(this);
    // }


    // #region Health and Death Code

    // public static event Action<Creature> OnHealthAdded = delegate {};
    // public static event Action<Creature> OnHealthRemoved = delegate {};
    // public event Action<float> OnHealthChanged = delegate { };


    // public void ModifyHealth(int amount)
    // {
    //     hp += amount;

    //     float hpPercent = (float)hp / MAX_HEALTH;
    //     OnHealthChanged(hpPercent); // update health bar

    //     if (hp <= 0) // check if dead
    //     {
    //         OnDeath();
    //     }
    // }


    protected virtual void OnDeath()
    {
        // What happens when something dies
        Destroy(gameObject);
    }

    // #endregion
}
