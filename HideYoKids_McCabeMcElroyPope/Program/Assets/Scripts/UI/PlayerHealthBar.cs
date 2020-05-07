using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private GameObject player;
    // private Health healthScript;
    private Player playerScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }
    
    void Update()
    {
        slider.value = playerScript.health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
