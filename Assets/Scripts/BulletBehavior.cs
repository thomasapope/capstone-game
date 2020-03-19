using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        Creature creature = other.gameObject.GetComponent<Creature>();
        if (creature)
        {
            print("Hit " + other.name + " for " + damage + " damage!");
            creature.TakeDamage(damage);
        }


        Destroy(gameObject);
    }
}
