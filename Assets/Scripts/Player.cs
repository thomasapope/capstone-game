using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    Player.cs

    A class meant to hold the behavioral code for player characters.
*/

[RequireComponent(typeof(CharacterController))]
public class Player : Creature
{
    // Component References
    private CharacterController controller;
    private Camera cam;
    [SerializeField] private Transform weaponPoint;
    public LayerMask interactableLayer;
    [HideInInspector] public WeaponSwitching weaponSwitching;
    [HideInInspector] public AlertArrowController alertArrowController;

    // [SerializeField]
    // private Animator animator;

    // Movement Stats
    public float RUNNING_SPEED = 10f;
    public float CARRYING_SPEED = 6f;
    [HideInInspector] public float movementSpeed = 10f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.08f;

    [HideInInspector] public bool usesGravity = true;
    public float gravity = 20f;

    protected Vector3 movementInput; // Input direction received from subclass
    private Vector3 moveDirection; // The desired movement direction

    private Vector3 targetVelocity; // The velocity we're aiming for
    private Vector3 velocity; // The current velocity
    private Vector3 smoothVelocity; // Used for velocity smoothing

    private Vector3 inputVector;
    private float x;
    private float z;

    // Weapons
    // public Weapon currentWeapon;
    public static event Action<Transform, AlertArrowController.AlertReason> PartPickedUp = delegate {};
    public static event Action PartDropped = delegate {};



    protected override void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        weaponSwitching = GetComponentInChildren<WeaponSwitching>();
        alertArrowController = GetComponentInChildren<AlertArrowController>();
        
        base.Start();
    }


    // Update is called once per frame
    protected override void Update()
    {
        //Check for interactable
        CheckForInteractable();

        if (!isCarryingItem) // Make sure the player can't attack while carrying an item
            hitting = Input.GetMouseButton(0); // Get attack input

        // if (hitting)
        // {
        //     animator.SetInteger("Weapon", 0);
        //     animator.SetInteger("AttackSide", 1);
        //     animator.SetInteger("Action", 2);
        //     animator.SetTrigger("AttackTrigger");
        // }

        if (Input.GetButtonDown("Attack"))
        {
            if (!currentWeapon.isRanged)
            {
                // if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack2"))
                // {
                //     animator.SetTrigger("attack3");
                //     hitsQueued++;
                // } else if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack1"))
                // {
                //     animator.SetTrigger("attack2");
                //     hitsQueued++;
                // } else
                // {
                //     animator.SetTrigger("attack1");
                //     hitsQueued++;
                // }
                // animator.SetTrigger("AttackTrigger");
                hitsQueued++;

                // hitsQueued++;
            }
            else
            {
                hitting = false;
                currentWeapon.Attack();
            }
        }
        else if (Input.GetButton("Attack"))
        {
            if (currentWeapon.isRanged)
            {
                hitting = false;
                currentWeapon.AttackContinuous();
            }
        }

        // Call the update method in the Creature class.
        base.Update();

        // Update the inputVector for movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        inputVector = transform.right * x + transform.forward * z;

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.z = Input.GetAxisRaw("Vertical");

        Move();
        
    }


    // Logic necessary for moving the creature.
    void Move() 
    {
        movementInput.Normalize();

        if (movementInput != Vector3.zero)
        {
            moveDirection = transform.right * movementInput.x + transform.forward * movementInput.z;
        }

        // Determine movement speed
        if (isCarryingItem)
            movementSpeed = CARRYING_SPEED; // Move slower when carrying something
        else
            movementSpeed = RUNNING_SPEED;



        if (moveDirection != Vector3.zero)
        {
            targetVelocity = movementSpeed * movementInput;
            velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothVelocity, speedSmoothTime);
        }

        Rotation(); // Rotation

        controller.Move(inputVector * velocity.magnitude * Time.deltaTime);

        animator.SetBool("Moving", true);
        
        animator.SetFloat("Velocity X", x);
        animator.SetFloat("Velocity Z", z);

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
    	Plane playerPlane = new Plane(Vector3.up, transform.position);
    	Ray ray = cam.ScreenPointToRay (Input.mousePosition);
        
    	float hitdist = 0.0f;
    	if (playerPlane.Raycast (ray, out hitdist)) 
		{
        	Vector3 targetPoint = ray.GetPoint(hitdist);
 
        	Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
 
        	transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
		}
    }


    void CheckForInteractable()
    {
        if(Input.GetMouseButtonUp(1)){
            // Drop the carried item if there is one
            if (isCarryingItem)
            {
                DropObject();
                // weaponSwitching.selectedWeapon = weaponSwitching.previousSelectedWeapon;
                // weaponSwitching.previousSelectedWeapon = -1;
                return;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable)
                {
                    // SetFocus(interactable);
                    if (Vector3.Distance(interactable.transform.position, transform.position) < pickupDistance)
                    {
                        if (!isCarryingItem) { // Make sure not to pick up more than one item
                            if (!interactable.pickedUp)
                            {
                                PickUpObject(interactable);
                                // weaponSwitching.selectedWeapon = -1;
                            }
                        }
                    }
                }
            }
        }
    }


    protected override void PickUpObject(Interactable obj)
    {
        base.PickUpObject(obj);
        weaponSwitching.selectedWeapon = -1;

        if (obj.isPart) // Is this a part?
        {
            PartPickedUp(GameManager.instance.escapeVehicleRef, AlertArrowController.AlertReason.Direction);
        }
    }


    protected override void DropObject()
    {
        base.DropObject();
        weaponSwitching.selectedWeapon = weaponSwitching.previousSelectedWeapon;
        weaponSwitching.previousSelectedWeapon = -1;
        
        PartDropped();
    }


    protected override void OnDeath()
    {
        // You died. Game over.
        Debug.Log("You Died");
        GameManager.instance.EndGame(); 

        Destroy(gameObject);
    }

}
