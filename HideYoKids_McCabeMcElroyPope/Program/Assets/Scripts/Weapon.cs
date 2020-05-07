using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundEffectVariation))]
public class Weapon : MonoBehaviour
{
    // Component references
    protected SoundEffectVariation audioSource;

    // [HideInInspector] public Transform attackPoint;
    // [HideInInspector] public LayerMask attackLayers;
    [HideInInspector] public Creature parent;
    [HideInInspector] public Vector3 target;

    // General stats
    public string name;
    public bool isRanged = false;
    public int damage = 10;
    
    // Melee stats
    public float attackRange = 0.5f;


    protected virtual void Start()
    {
        audioSource = GetComponent<SoundEffectVariation>();
    }




    public virtual void Attack()
    {
        audioSource.PlayVaryPitch();

        // hitting = false;
        // hitsQueued--;

        Collider[] hits = Physics.OverlapSphere(parent.attackPoint.position, attackRange, parent.attackLayers);
        
        if(hits.Length == 0) return;

        foreach (Collider enemy in hits)
        {
            enemy.GetComponent<Creature>().TakeDamage(damage);
            print("Hit " + enemy.name + " for " + damage + " damage!");
        }
    
    }

    
    public virtual void AttackContinuous()
    {

    }
}
