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
[RequireComponent(typeof(Health))]
public class Enemy : Creature
{
    public Transform target;
    private UnityEngine.AI.NavMeshAgent agent;


    protected virtual void Start()
    {
        base.Start();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    protected override void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        agent.SetDestination(target.position);

        if (distance <= agent.stoppingDistance)
        {
            // Attack
            //hitting = true;
            FaceTarget();
        }

        base.Update();
    }


    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
    }


    protected override void OnDisable()
    {
        GameStats.score++; // Add one kill to the score
        base.OnDisable();
    }
}
