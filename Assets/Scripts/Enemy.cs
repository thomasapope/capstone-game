using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    Enemy.cs

    The base class for simple enemies. Includes pathfinding behaviors.
*/


[RequireComponent(typeof(NavMeshAgent))]
// [RequireComponent(typeof(CharacterController))]
// [RequireComponent(typeof(Health))]
public class Enemy : Creature
{
    public Transform startingPoint;
    public Transform target;
    // public Transform carryPoint;
    private UnityEngine.AI.NavMeshAgent agent;

    public enum MindState { CHASING, FLEEING }
    private MindState state = MindState.CHASING;

    public float refreshTime = 5f;
    private float timeTilRefresh;


    protected virtual void Start()
    {
        base.Start();

        // attackRate = .5f;

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

        float distance = Vector3.Distance(target.position, transform.position);

        agent.SetDestination(target.position);

        // Check if enemy is close to target
        if (distance <= agent.stoppingDistance)
        {
            if (target.CompareTag("Target"))
            {
                if (GameManager.numOfChildren < 1 || target.parent.gameObject.GetComponent<Interactable>().pickedUp == true)
                {
                    FindTarget();
                    return;
                }

                Debug.Log("They're taking the children!!!");
                state = MindState.FLEEING;
                // GameObject child = Instantiate(GameManager.childPrefab, carryPoint.position, target.rotation);
                // child.transform.SetParent(transform); // Pick up child
                GameManager.numOfChildren--;

                PickUpObject(target.transform.parent.gameObject.GetComponent<Interactable>());

                // target.SetParent(transform);
            }
            if (target.CompareTag("SpawnPoint")) 
            {

            }

            // Attack
            hitting = true;
            FaceTarget();
        }

        base.Update();
    }


    private void FindTarget()
    {
        // Finds a new target for the enemy. 
        // Should not be executed every frame as it is resource intensive.
        if (state == MindState.CHASING)
        {
            if (GameManager.numOfChildren < 1)
            {
                target = GameManager.playerRef.transform;
                return;
            }

            float minDistance = 1000f;
            Transform closest = transform;
            foreach (GameObject g in GameManager.targetRefs)
            {
                float dist = Vector3.Distance(transform.position, g.transform.position);
                if (dist < minDistance) 
                {
                    minDistance = dist;
                    closest = g.transform;
                }
            }
            if (closest)
            {
                // Debug.Log(closest.gameObject.name);
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


    protected override void OnDeath()
    {
        Debug.Log("Enemy slain!!!");

        // Unparent child if carrying
        if (state == MindState.FLEEING)
        {
            // Debug.Log("Your enemies have dropped a child!!!");
            // Debug.Log(target.name);
            // Transform kid = transform.GetChild(0).gameObject.GetComponentInChildren<Transform>();
            // if (kid.CompareTag("Target"))
            // {
            //     kid.SetParent(null);
            // }


            Transform[] children = GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child.CompareTag("holdable"))
                {
                    child.SetParent(null);
                }
            }

            // target.SetParent(null);
            // target.parent = null;
        }

        Destroy(gameObject);
    }

    protected override void OnDisable()
    {
        GameStats.score++; // Add one kill to the score
        base.OnDisable();
    }
}
