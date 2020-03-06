using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public bool pickedUp = false;
    public GameObject objectUI;
    public string itemName;
    public string itemDescription;

    public float radius = .5f;
    

    void Update()
    {
        if(GameManager.playerRef){
            if(Vector3.Distance(GameManager.playerRef.transform.position, transform.position) < 4 && !pickedUp){
                objectUI.SetActive(true);
            }else{
                objectUI.SetActive(false);
            }
        }
        
    }

    public virtual void OnPickUp()
    {
        pickedUp = true;
        // Debug.Log(pickedUp);
        // objectUI.SetActive(false);
    }


    public virtual void OnDrop()
    {
        pickedUp = false;
    }


    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, radius);
    // }
}
