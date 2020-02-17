﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStats : MonoBehaviour
{
    public static event Action<int> OnScoreChanged = delegate {};

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
        scoreLabel.text = "Kills: " + score + "\n" + 
                            "Damage: " + damage;
    }
    
    public static void ModifyScore(int score)
    {
        GameStats.score += score;

        OnScoreChanged(GameStats.score);
    }
    
}
