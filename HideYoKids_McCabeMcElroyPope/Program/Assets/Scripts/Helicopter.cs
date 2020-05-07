
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    private LayerMask interactableLayer;
    public List<GameObject> interactablesNear;
    
    public event Action<float> OnHealthChanged = delegate {};


    void Start()
    {
        interactableLayer = LayerMask.GetMask("Interactable");
    }


    void Update()
    {
        // Check for dropped items
        if (GameManager.playerRef)
        {
            float distance = Vector3.Distance(GameManager.playerRef.transform.position, this.transform.position);
            if(distance < 10){
                
                Collider[] hits = Physics.OverlapSphere(this.transform.position, 10, interactableLayer);
                foreach(Collider item in hits){
                    if(item.gameObject != null  && !item.gameObject.GetComponent<Interactable>().pickedUp){
                        // if(!interactablesNear.Contains(item.gameObject) && (item.gameObject.GetComponent<Child>() == null)){
                        if(!interactablesNear.Contains(item.gameObject)) // Make sure the item isn't already present
                        { 

                            if (interactablesNear.Count < GameManager.instance.numOfParts && item.gameObject.GetComponent<Child>())
                            {
                                // If this is a child and there are still parts to be found, do nothing.
                                break;
                            }
                            else
                            {
                                // If it is not a child or all parts have been found, add the item
                                AddInteractable(item.gameObject);
                                if (item.gameObject.GetComponent<Child>())
                                {
                                    GameStats.childrenSaved++;
                                }
                            }
                        }

                    }
                }
            }
        }

        if(interactablesNear.Count == GameManager.instance.numOfParts + GameManager.numOfChildren)
        {
            GameManager.isVictory = true;
            GameManager.instance.EndGame();
        }
        
    }


    void AddInteractable(GameObject item)
    {
        interactablesNear.Add(item);
        item.GetComponent<Interactable>().objectUI.SetActive(false);
        GameObject.Destroy(item);
    }
}
