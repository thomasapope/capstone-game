using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public bool pickedUp = false;
    public bool isPart = true;
    public GameObject objectUI;
    public Rigidbody rigidbody;
    public string itemName;
    public string itemDescription;

    public float radius = .5f;


    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
        // rigidbody = GetComponent<Rigidbody>();
    }
    

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
        print("picking up");
        pickedUp = true;
        if (rigidbody)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }


    public virtual void OnDrop()
    {
        pickedUp = false;
        if (rigidbody)
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }
}
