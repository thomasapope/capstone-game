using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    // Component references
    protected AudioSource audioSource;

    // General stats
    public string name;
    public bool isRanged = false;
    public int damage = 10;


    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }




    public virtual void Attack()
    {

    }

    
    public virtual void AttackContinuous()
    {

    }
}
