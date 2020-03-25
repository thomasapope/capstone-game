using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    Enemy.cs

    The base class for simple enemies. Includes pathfinding behaviors.
*/


[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Creature
{
    [HideInInspector] public Transform startingPoint;
    [HideInInspector] public Transform target;
    private UnityEngine.AI.NavMeshAgent agent;

    public enum MindState { CHASING, FLEEING }
    private MindState state = MindState.CHASING;

    public float refreshTime = 3f;
    private float timeTilRefresh;


    protected override void Start()
    {
        base.Start();

        // print(currentWeapon);

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    protected override void Update()
    {
        // Find Target
        if (!target) FindTarget();

        if (state == MindState.FLEEING) target = startingPoint;

        if (state == MindState.CHASING) 
        {
            if (timeTilRefresh < 0) // A timer to keep this from getting calculated every frame
            {
                FindTarget();

                timeTilRefresh = refreshTime;
            }
            else
            {
                timeTilRefresh -= Time.deltaTime;
            }
        }

        agent.SetDestination(target.position); // start the enemy moving toward its target

        // Check if enemy is close to target
        if (hasReachedTarget())
        {
            if (target.CompareTag("Target")) // If the target is a child
            {
                if (target.parent.gameObject.GetComponent<Interactable>().pickedUp || !target.gameObject.activeInHierarchy)
                {
                    Debug.Log("My target is already taken");
                    FindTarget();
                    return;
                }

                state = MindState.FLEEING;

                PickUpObject(target.parent.gameObject.GetComponent<Interactable>());
            }

            if (target.CompareTag("SpawnPoint")) // If the enemy has returned to their spawn point
            {
                GameManager.NumOfChildren--;
                Destroy(gameObject);
            }

            // Attack
            hitting = true;
            FaceTarget();
        }

        base.Update();
    }


    bool hasReachedTarget()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return distance <= agent.stoppingDistance;
    }


    private void FindTarget()
    {
        // Finds a new target for the enemy. 
        // Should not be executed every frame as it is resource intensive.
        if (state == MindState.CHASING)
        {
            // if (target)
            // {
            //     if(!target.gameObject.activeInHierarchy)
            //     {
            //         target = GameManager.playerRef.transform;
            //         return;
            //     }
            // }

            float minDistance = 1000f;
            Transform closest = transform;
            foreach (GameObject g in GameManager.targetRefs)
            {
                if (!g) continue;
                if (!g.gameObject.activeInHierarchy) continue;
                // Child child = g.GetComponent<Child>();
                // if (child)
                // {
                //     if (child.state == Child.ChildState.HIDDEN) 
                //     {
                //         print("this is true");
                //         continue;
                //     }
                // }
                float dist = Vector3.Distance(transform.position, g.transform.position);
                if (dist < minDistance) 
                {
                    minDistance = dist;
                    closest = g.transform;
                }
            }
            if (closest)
            {
                target = closest;
            }
            else 
            {
                return;
            }
        }
    }


    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        GameManager.damage += damage;
    }


    protected override void OnDeath()
    {
        // Unparent child if carrying
        if (state == MindState.FLEEING)
        {
            DropObject();

            Debug.Log("Your enemies have dropped a child!!!");
        }

        GameManager.kills++; // Add one kill to the score

        Destroy(gameObject);
    }


    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
