using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public bool pickedUp = false;
    public bool isPart = true;
    public GameObject objectUI;
    public string itemName;
    public string itemDescription;

    public float radius = .5f;
    

    protected virtual void Update()
    {
        if(GameManager.playerRef){
            if(Vector3.Distance(GameManager.playerRef.transform.position, transform.position) < 4f && !pickedUp){
                objectUI.SetActive(true);
            }else{
                objectUI.SetActive(false);
            }
        }
        
    }


    public virtual void OnPickUp()
    {
        pickedUp = true;
    }


    public virtual void OnDrop()
    {
        pickedUp = false;
    }
}
