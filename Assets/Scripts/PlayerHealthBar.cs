using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    private GameObject player;
    private Health healthScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthScript = player.GetComponent<Health>();
    }
    
    void Update()
    {
        slider.value = healthScript.hp;
    }
}
