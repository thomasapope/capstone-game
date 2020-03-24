using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    LayerMask interactableLayer;
    void Start(){
        interactableLayer = LayerMask.GetMask("Interactable");
    }
    public List<GameObject> interactablesNear;
    void Update()
    {
        if (GameManager.playerRef)
        {
            float distance = Vector3.Distance(GameManager.playerRef.transform.position, this.transform.position);
            if(distance < 10){
                
                Collider[] hits = Physics.OverlapSphere(this.transform.position, 10, interactableLayer);
                foreach(Collider item in hits){
                    if(item.gameObject != null  && !item.gameObject.GetComponent<Interactable>().pickedUp){
                        if(!interactablesNear.Contains(item.gameObject) && (item.gameObject.GetComponent<Child>() == null)){
                            interactablesNear.Add(item.gameObject);
                            GameObject.Destroy(item.gameObject);
                        }

                    }
                }
            }
        }

        if(interactablesNear.Count == 3){
            GameManager.isVictory = true;
            GameManager.instance.EndGame();
        }
        
    }
}
