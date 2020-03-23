/*
    AlertArrow.cs

    Used to point the direction of important events, such
    as an enemy picking up and carrying away a child.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertArrowController : MonoBehaviour
{
    public GameObject arrowPrefab;


    void Start()
    {
        // Create new alert arrow when the delegate gets called
        Child.ChildPickedUp += CreateAlertArrow;


        // Creature.OnHealthAdded += AddHealthBar;
        // Creature.OnHealthRemoved += RemoveHealthBar;
        // arrow = 

        // bullet.transform.position = firePoint.position;
        // Vector3 rotation = bullet.transform.rotation.eulerAngles;
        // bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
    }


    public void CreateAlertArrow(Transform location)
    {
        GameObject arrow = Instantiate(arrowPrefab); // create the arrow
        arrow.transform.SetParent(transform); // reparent it so it moves along with the player
        arrow.transform.position = transform.position; // position it properly
        arrow.GetComponent<AlertArrow>().target = location;

    }
}
