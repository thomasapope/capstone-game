using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    bool pickedUp;
    Player player;
    public GameObject objectUI;
    public string itemName;
    public string itemDescription;
    void Start()
    {
        pickedUp = false;
        player = GameObject.FindObjectOfType<Player>();
        player.OnItemPickUp += PickUpObject;
    }


    void Update()
    {
        if(player != null){
            if(Vector3.Distance(player.transform.position, transform.position) < 4 && !pickedUp){
                objectUI.SetActive(true);
            }else{
                objectUI.SetActive(false);
            }
        }
        
    }

    void PickUpObject()
    {
        pickedUp = true;
        gameObject.SetActive(false);
    }
}
