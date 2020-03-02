using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStats : MonoBehaviour
{
    public static int score { get; set; }
    public static float damage { get; set; }

    [SerializeField]
    private Text scoreLabel;


    void Start()
    {
        scoreLabel = GetComponent<Text>();
    }


    void Update()
    {
        scoreLabel.text = "Wave: " + (WaveSpawner.nextWave + 1) + "\n" + 
                            "Kills: " + score + "\n" + 
                            "Damage: " + damage + "\n" + 
                            "Children Left: " + GameManager.numOfChildren;
    }
}
