using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // General stats
    public string name;
    public bool isRanged;
    public float damage = 10f;

    // Only for ranged weapons
    public GameObject projectile;
    public Transform firePoint; // Where the bullets are created
    public float fireRate = 0.5f;



    void Start()
    {
        if (isRanged)
        {
            if (!projectile)
                Debug.Log("No projectile assigned to ranged weapon. Please assign a projectile.");
            if (!firePoint)
                Debug.Log("No firePoint designated. Please specify a firepoint for " + name);
        }
    }
}
