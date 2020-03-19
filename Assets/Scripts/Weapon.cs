using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // General stats
    public string name;
    public bool isRanged = false;
    public float damage = 10f;


    public virtual void Attack()
    {
        
    }
}
