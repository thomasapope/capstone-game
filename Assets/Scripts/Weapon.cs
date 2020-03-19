using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundEffectVariation))]
public class Weapon : MonoBehaviour
{
    // Component references
    protected SoundEffectVariation audioSource;

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
    }

    
    public virtual void AttackContinuous()
    {

    }
}
