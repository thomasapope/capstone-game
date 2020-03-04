﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    bool pickedUp = false;
    // Player player;
    public GameObject objectUI;
    public string itemName;
    public string itemDescription;
    
    
    // protected virtual void Start()
    // {
    //     pickedUp = false;
    //     // player = GameObject.FindObjectOfType<Player>();
    // }


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

    public virtual void PickUpObject()
    {
        pickedUp = true;
        // Debug.Log(pickedUp);
    }
}
