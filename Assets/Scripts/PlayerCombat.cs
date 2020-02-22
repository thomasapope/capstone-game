using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator attackAnimator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public float attackRate = 2f;

    private float nextAttackTime = 0f;


    void Update()
    {
        // if (Time.time >= nextAttackTime)
        // {
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         Attack();
        //         nextAttackTime = Time.time + 1f / attackRate;
        //     }
        // }
    }

    void Attack()
    {
        attackAnimator.SetTrigger("swing");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Creature>().TakeDamage(50);
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
