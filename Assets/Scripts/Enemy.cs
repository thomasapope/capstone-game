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

    public enum MindState { CHASING, ATTACKING, FLEEING }
    [SerializeField] private MindState state = MindState.CHASING;

    [HideInInspector] public float refreshTime = 2f;
    private float timeTilRefresh;

    public float attackDistance = 2f;
    public int weapon = 0;

    public float attackTime = 2f; // Amount of time it takes to complete an attack and return to previous state


    protected override void Start()
    {
        base.Start();

        animator.SetInteger("weapon", weapon);
        // currentWeapon.attackPoint = attackPoint;
        // currentWeapon.attackLayers = 
        currentWeapon.parent = this;
        // print(currentWeapon);
        
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if(!startingPoint)
        {
            startingPoint = WaveSpawner.instance.spawnPoints[0];
            Debug.Log("No starting point set. Setting new one");
        }

    }


    protected override void Update()
    {
        // Find Target
        if (!target) FindTarget();

        switch (state)
        {
            case MindState.FLEEING:
                target = startingPoint;
                agent.SetDestination(target.position); // start the enemy moving toward its target
                animator.SetBool("moving", true);

                if (HasReachedTarget())
                {
                    if (target.CompareTag("SpawnPoint")) // If the enemy has returned to their spawn point
                    {
                        GameManager.NumOfChildren--;
                        Destroy(gameObject);
                    }
                }
                break;
            case MindState.CHASING:
                if (timeTilRefresh < 0) // A timer to keep this from getting calculated every frame
                {
                    FindTarget();

                    timeTilRefresh = refreshTime;
                }
                else
                {
                    timeTilRefresh -= Time.deltaTime;
                }

                agent.SetDestination(target.position); // start the enemy moving toward its target
                animator.SetBool("moving", true);

                // Check if target is child and enemy is close enough to pick it up
                if (target.CompareTag("Target") && HasReachedTarget())
                {
                    if (target.parent.gameObject.GetComponentInChildren<Interactable>().pickedUp || !target.gameObject.activeInHierarchy)
                    {
                        Debug.Log("My target is already taken");
                        FindTarget();
                        return;
                    }

                    state = MindState.FLEEING;

                    PickUpObject(target.parent.gameObject.GetComponentInChildren<Interactable>());
                } 
                else if (HasReachedAttackTarget()) // If the enemy is close enough to attack
                {
                    if (Time.time >= nextAttackTime)
                    {
                        // Make sure the enemy has a line of sight to the player
                        // if (!Physics.Linecast(transform.position, target.position, attackLayers))
                        // if (!Physics.Linecast(transform.position, target.position))
                        {
                            // Play attack animation
                            animator.SetTrigger("attack");
                            animator.SetBool("moving", false);

                            // Resest navmesh path
                            agent.ResetPath();

                            // Change to attacking state
                            // Stops enemy from moving while attacking
                            nextAttackTime = Time.time + attackTime; // timer for returning to chase state
                            state = MindState.ATTACKING;

                            // Do the actual attacking
                            StartCoroutine(Attack(attackDelay));

                            IEnumerator Attack(float delay)
                            {
                                yield return new WaitForSeconds(delay);

                                currentWeapon.target = target.transform.position + Vector3.up * 2;
                                currentWeapon.Attack();
                            }
                        }
                    }
                    FaceTarget();
                }
                break;
            case MindState.ATTACKING:
                // Make sure to properly face the target
                FaceTarget();

                // Return to chasing state when appropriate
                if (Time.time >= nextAttackTime)
                {
                    state = MindState.CHASING;
                    // nextAttackTime = Time.time + 1f / attackRate; // reset time for next attack
                }
                break;
        } // end switch

        base.Update();
    }


    bool HasReachedTarget()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return distance <= agent.stoppingDistance;
    }


    bool HasReachedAttackTarget()
    {
        if (!target.CompareTag("Player")) return false;
        if (target.GetComponent<Creature>().health < 0) return false; // Don't attack when the player is dead
        float distance = Vector3.Distance(target.position, transform.position);
        return distance <= attackDistance;
    }


    private void FindTarget()
    {
        // Finds a new target for the enemy. 
        // Should not be executed every frame as it is resource intensive.
        if (state == MindState.CHASING)
        {

            float minDistance = 1000f;
            Transform closest = transform;
            foreach (GameObject g in GameManager.targetRefs)
            {
                if (!g) continue;
                if (!g.gameObject.activeInHierarchy) continue;

                // print("target: " + g.name);

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
