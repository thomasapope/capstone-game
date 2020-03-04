﻿using System.Collections;
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
    [SerializeField]
    private Transform weaponPoint;
    // [SerializeField]
    // private Animator animator;

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

    // Picked Up Items
    public List<Interactable> pickedUpItems;
    public LayerMask interactableLayer;


    // Weapons
    [System.Serializable]
    public class Weapon
    {
        public string name;
        public GameObject prefab;
    }

    private int currentWeapon = 0;
    public Weapon[] weapons;
    private bool weaponIsSwitching = false;

    


    // public override void Initialize()
    protected virtual void Start()
    {
        // movementSpeed = 10f;
        // attackDamage = 35;

        controller = GetComponent<CharacterController>();
        cam = Camera.main;

        // SwitchWeapon(currentWeapon);
        
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Check for interactable
        CheckForInteractable();
        // hitting = Input.GetKeyDown(KeyCode.Space);
        hitting = Input.GetMouseButton(0); // Get attack input

        //WeaponSelection();

        // Call the update method in the Creature class.
        base.Update();

        // Update the inputVector for movement
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
            // Debug.Log("Hit Button");
            Collider[] hits = Physics.OverlapSphere(attackPoint.position, 4, interactableLayer);
        
            if(hits.Length == 0) return;

            foreach (Collider item in hits)
            {
                GameObject objectToDestroy = item.gameObject;
                Interactable interactableItem = item.GetComponent<Interactable>();
                PickUpItem(interactableItem);
                objectToDestroy.SetActive(false);

            }
        }
    }
    // Add Interactable to PickedUp List.
    void PickUpItem(Interactable item) 
    {
        pickedUpItems.Add(item);
        // foreach(int i in pickedUpItems)
        // {

        // }
    }


    
}
