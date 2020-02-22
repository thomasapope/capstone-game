using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    Creature.cs

    The superclass of all living creatures. This is not meant to be used
    as is, but to be inherited from.
*/

[RequireComponent(typeof(Health))]
public abstract class Creature : MonoBehaviour
{
    // Component References
    public Health stats;
    Renderer rend;
    
    // Other Stats
    private float hitTime = 1f;
    private Material defMat;
    public static Material hitMat;

    // Attack variables
    protected bool hitting;
    protected int attackDamage = 10;

    public Animator attackAnimator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask attackLayers; // The layers this creature can deal damage to

    public float attackRate = 2f;

    private float nextAttackTime = 0f;


    protected virtual void Start()
    {
        stats = gameObject.GetComponent<Health>();
        rend = GetComponent<Renderer> ();
        defMat = rend.material;

        if (hitMat == null)
            hitMat = Resources.Load<Material>("HitMat");
    }
    
    
    protected virtual void Update()
    {
        // Attacking
        if (Time.time >= nextAttackTime)
        {
            if (hitting)
            {
                // Debug.Log("Hitting");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        // Hit feedback
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
    }


    void Attack()
    {
        hitting = false;

        if (attackAnimator != null)
        {
            attackAnimator.SetTrigger("swing");
        }

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, attackLayers);
        
        if(hits.Length == 0) return;

        foreach (Collider enemy in hits)
        {
            enemy.GetComponent<Creature>().TakeDamage(attackDamage);
        }
    }


    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage!");
        stats.ModifyHealth(damage * -1);
        
        hitTime = 0;
        rend.material = hitMat;
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "sword")
        {
            // stats.ModifyHealth(-10);
            GameStats.damage += 10;
            Debug.Log("Sword hit");
            
            hitTime = 0;
            rend.material = hitMat;
        }
        if (hit.gameObject.name == "EnemyAttack")
        {
            // stats.ModifyHealth(-1);
            Debug.Log("Enemy hit");
            
            hitTime = 0;
            rend.material = hitMat;
        }

    }

    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
