// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    // float currentTime;
    // float startingTime = 15f;

    float time;

    float quickTime = 10f;
    float urgentTime = 5f;
    

    [SerializeField]
    private Text timer;

    // private Color invisible = new Color(0, 0, 0, 0);
    public Color defColor = Color.white;
    public Color quickColor = Color.yellow;
    public Color urgentColor = Color.red;


    void Start()
    {
        timer = GetComponent<Text>();
        // currentTime = startingTime;
        timer.color = defColor;

        WaveSpawner.WaveComplete += OnWaveCompleted;
        WaveSpawner.WaveStarting += OnWaveStarting;
    }


    void Update()
    {
        time = WaveSpawner.waveCountdown;

        timer.text = time.ToString(); // Update timer text

        // Change color when time is almost up
        if (WaveSpawner.state == WaveSpawner.SpawnState.COUNTING)
        {
            if (time <= quickTime)
            {
                timer.color = quickColor;
            }
            if (time <= urgentTime)
            {
                timer.color = urgentColor;
            }
        }
    }


    void OnWaveCompleted()
    {
        // Make the timer visible unless all waves are complete
        if (!WaveSpawner.complete)
        {
            timer.color = defColor;
        }
    }


    void OnWaveStarting()
    {
        timer.color = new Color(0, 0, 0, 0);
    }
}
