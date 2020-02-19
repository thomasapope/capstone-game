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
    private Transform mainCameraTransform = null;


    // Movement Stats
    public float movementSpeed = 10f;
    private float currentSpeed;
    private float speedSmoothVelocity;
    private float speedSmoothTime = 0.2f;
    private float rotationSpeed = 0.03f;
    
    public bool usesGravity = true;
    public float gravity = 10f;

    // private bool isGrounded;

    private float hitTime = 1f;
    Renderer rend;
    private Material defMat;
    public static Material hitMat;

    public Health stats;


    protected Vector3 inputVector;
    private Vector3 movementInput;
    private Vector3 lookDirection;
    private Vector3 moveDirection; // the current movement direction and speed


    // Initialize method must be overriden to update variables such as speed and health.
    public abstract void Initialize();


    protected virtual void Start()
    {
        controller = this.GetComponent<CharacterController>(); // Get the CharacterController at runtime
        mainCameraTransform = Camera.main.transform;

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
    // Uses the inputVector given it by the subclass and moves based on it.
    // This allows us to create movement behavior once and reuse it for 
    // most or all players and enemies.
    void Move() 
    {
        movementInput = new Vector3(inputVector.x, 0, inputVector.z).normalized;

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;


        if (movementInput != Vector3.zero)
        {
            moveDirection = new Vector3(movementInput.x, 0, movementInput.z).normalized;
        }
        // Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;
        

        float targetSpeed = movementSpeed * movementInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        Rotation(currentSpeed); // Rotation

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

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


    void Rotation(float speed)
    {
        if (movementInput != Vector3.zero)
        {
            // Vector3 rotation = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // transform.LookAt(transform.position + rotation);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
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
