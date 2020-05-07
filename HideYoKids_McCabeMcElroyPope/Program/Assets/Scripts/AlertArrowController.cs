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

    public Material warningMat;
    public Material directionMat;

    public enum AlertReason { Warning, Direction }

    public int numOfArrows = 0; // Number of arrows. Used for positioning
    private float arrowDistance = .25f; // Distance

    public List<AlertArrow> arrows;


    void Start()
    {
        // Create new alert arrow when the delegate gets called
        Child.ChildPickedUp += CreateAlertArrow;
        Player.PartPickedUp += CreateAlertArrow;
    }


    public void CreateAlertArrow(Transform location)
    {
        CreateAlertArrow(location, AlertReason.Warning);
    }


    public void CreateAlertArrow(Transform location, AlertReason reason)
    {
        
        // Instantiate and reparent arrow
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.SetParent(transform);

        // Position arrow
        Vector3 arrowPosition = transform.position;
        arrowPosition.y += arrowDistance * numOfArrows;
        arrow.transform.position = arrowPosition;
        PositionArrow(arrow, numOfArrows);

        // Set arrow settings
        AlertArrow aArrow = arrow.GetComponent<AlertArrow>();
        aArrow.target = location; // set arrow target
        aArrow.reason = reason; // set the reason for the alert arrow
        aArrow.controllerRef = this; // set callback reference

        // Set arrow material (color)
        Renderer arrowRenderer = arrow.GetComponentInChildren<Renderer>();
        if (reason == AlertReason.Warning)
        {
            arrowRenderer.material = warningMat;
        }
        else
        {
            arrowRenderer.material = directionMat;
        }
        
        // Increment arrow count
        numOfArrows++;
        arrows.Add(aArrow);
    }


    
    public int NumOfArrows
    {
        get { return numOfArrows; }
        set 
        { 
            numOfArrows = value; 

            for(int i = 0; i < arrows.Count; i++)
            {
                PositionArrow(arrows[i].gameObject, i);
            }
            
            // foreach (AlertArrow a in arrows)
            // {
                
            // }
        }
    }


    void PositionArrow(GameObject arrow, int position)
    {
        Vector3 arrowPosition = transform.position;
        arrowPosition.y += arrowDistance * position;
        arrow.transform.position = arrowPosition;
    }


    void OnDisable() 
    {
        Child.ChildPickedUp -= CreateAlertArrow;
    }
}
