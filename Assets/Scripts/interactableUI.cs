using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class interactableUI : MonoBehaviour
{
    public Transform cam;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    private Interactable item;
    void Start()
    {
        cam = Camera.main.transform;
        item = GetComponentInParent<Interactable>();
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;
        
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - cam.transform.position).normalized);
    }
}
