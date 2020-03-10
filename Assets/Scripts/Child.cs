﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Child : Interactable
{
    // Component references
    public GameObject targetRef;
    public LayerMask enemyLayer;
    public LayerMask stashLayer;
    private Transform target;
    private NavMeshAgent agent;

    // States
    public enum ChildState {HIDDEN, WANDERING, RUNNING, CARRIED, SAFE}
    public ChildState state = ChildState.WANDERING;

    // Messages
    public static event Action<Transform> ChildLeftHiding = delegate {};

    public static event Action<Transform> ChildPickedUp = delegate {};
    public static event Action<Transform> ChildTaken = delegate {};


    // Behavior timing
    [SerializeField]
    private float timer;

    public float wanderRadius = 20f;
    public float hiddenTime = 15f; // seconds before child will leave hiding
    public float wanderTime = 10f; // seconds before wandering in a new direction
    public float safeTime = 3f; // seconds a child is safe after being dropped
 
    public float enemyDetectionRadius = 5f;
    public float stashRadius = 1f; // the radius that is checked to determine if the child has been hidden in a stash

    // Use this for initialization
    void OnEnable () 
    {
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTime / 2; // Set initial wander time
    }
 

    // Update is called once per frame
    protected override void Update () 
    {
        base.Update();

        // Update the timer. Used in multiple states
        timer += Time.deltaTime;

        switch (state)
        {
            case ChildState.HIDDEN: // Wait for a time before going back to wandering. Occurs when hidden.
                if (timer >= hiddenTime)
                {
                    ChangeState(ChildState.WANDERING);
                }
                break;

            case ChildState.WANDERING: // Wander around
                // Check if there is an enemy nearby
                if (Physics.CheckSphere(transform.position, enemyDetectionRadius, enemyLayer, QueryTriggerInteraction.Collide))
                {
                    // ChangeState(ChildState.HIDDEN);
                    print("ENEMY NEARBY!!!");
                }

                // Track time until direction change
                if (timer >= wanderTime) {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
                break;

            case ChildState.SAFE: // Wait for a time before going back to wandering. Occurs when dropped.
                if (timer >= safeTime)
                {
                    ChangeState(ChildState.WANDERING);
                }
                break;

            default:
                break;
        }
    }


    void ChangeState(ChildState newState)
    {
        // Changes the state the child is in and runs one time code for that state
        state = newState;
        // print(newState);

        switch (newState)
        {
            case ChildState.WANDERING:
                timer = wanderTime;
                targetRef.SetActive(true);
                break;
            case ChildState.CARRIED:
                agent.ResetPath();
                agent.enabled = false; // disable navmeshagent to keep the child from walking away while being carried
                break;
            case ChildState.HIDDEN:
            case ChildState.SAFE:
                timer = 0f;
                targetRef.SetActive(false);
                break;
            default:
                timer = 0f;
                break;
        }
    }

 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }


    public override void OnPickUp()
    {
        base.OnPickUp();

        ChangeState(ChildState.CARRIED);

        // Check if being picked up by an enemy
        if (transform.parent.parent.gameObject.CompareTag("Enemy"))
        {
            ChildPickedUp(transform);
        }

        targetRef.SetActive(false);
    }
    

    public override void OnDrop()
    {
        base.OnDrop();

        // If being dropped in a stash, hide the child
        if (Physics.CheckSphere(transform.position, stashRadius, stashLayer, QueryTriggerInteraction.Collide))
        {
            ChangeState(ChildState.HIDDEN);
        }
        else
        {
            ChangeState(ChildState.SAFE);
        }

        agent.enabled = true; // re-enable the navmeshagent (this also drops the child to the ground)

        // targetRef.SetActive(true);
    }


    void OnDisable()
    {
        if (GameManager.targetRefs.Contains(gameObject))
        {
            GameManager.targetRefs.Remove(gameObject);
        }

        ChildTaken(transform);
    }

}
