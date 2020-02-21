using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    Creature.cs

    The superclass of all creatures and players. This is not meant to be used
    as is, but to be inherited from.
*/

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Health))]
public abstract class Creature : MonoBehaviour
{
    // Component References
    private CharacterController controller;

    // Movement Stats
    public float movementSpeed = 10f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.08f;

    public bool usesGravity = true;
    public float gravity = 10f;

    protected Vector3 movementInput; // Input direction received from subclass
    private Vector3 moveDirection; // The desired movement direction

    private Vector3 targetVelocity; // The velocity we're aiming for
    private Vector3 velocity; // The current velocity
    private Vector3 smoothVelocity; // Used for velocity smoothing
    
    // Other Stats
    private float hitTime = 1f;
    Renderer rend;
    private Material defMat;
    public static Material hitMat;

    public Health stats;


    // protected Vector3 inputVector; 


    // Initialize method must be overriden to update variables such as speed and health.
    public abstract void Initialize();


    protected virtual void Start()
    {
        controller = this.GetComponent<CharacterController>(); // Get the CharacterController at runtime

        stats = gameObject.GetComponent<Health>();
        rend = GetComponent<Renderer> ();
        defMat = rend.material;

        if (hitMat == null)
            hitMat = Resources.Load<Material>("HitMat");

        // Run Initialize method as defined in subclass
        Initialize();
    }
    
    
    protected virtual void Update()
    {
        if (hitTime < 1)
        {
            hitTime += Time.deltaTime;
        }
        else
        {
            if (rend.material != defMat)
            {
                rend.material = defMat;
            }
        }
        
        Move();
    }


    // Logic necessary for moving the creature.
    void Move() 
    {
        movementInput.Normalize();

        if (movementInput != Vector3.zero)
        {
            moveDirection = new Vector3(movementInput.x, 0, movementInput.z).normalized;
        }

        if (moveDirection != Vector3.zero)
        {
            targetVelocity = movementSpeed * movementInput;
            velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothVelocity, speedSmoothTime);
        }

        Rotation(); // Rotation

        controller.Move(velocity * Time.deltaTime);

        if (usesGravity) 
        {
            Gravity();
        }
    }


    // Adds a downward force to the creature
    void Gravity()
    {
        Vector3 gravityVector = Vector3.zero;
        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }
        
        controller.Move(gravityVector * Time.deltaTime);
    }


    void Rotation()
    {
        if (movementInput != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), rotationSpeed);
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "sword")
        {
            stats.ModifyHealth(-10);
            GameStats.damage += 10;
            Debug.Log("Sword hit");
            
            hitTime = 0;
            rend.material = hitMat;
        }
        if (hit.gameObject.name == "EnemyAttack")
        {
            stats.ModifyHealth(-1);
            Debug.Log("Enemy hit");
            
            hitTime = 0;
            rend.material = hitMat;
        }

    }
}
