using System.Collections;
using System.Collections.Generic;
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

    public Color defColor = Color.white;
    public Color quickColor = Color.yellow;
    public Color urgentColor = Color.red;


    void Start()
    {
        timer = GetComponent<Text>();
        // currentTime = startingTime;
        timer.color = defColor;
    }


    void Update()
    {
        time = WaveSpawner.waveCountdown;

        timer.text = time.ToString(); // Update timer text

        // Change color when time is almost up
        if (time <= quickTime) {
            timer.color = quickColor;
        }
        if (time <= urgentTime) {
            timer.color = urgentColor;
        }

        // Control the visibility of the timer
        if (time <= 0 || WaveSpawner.complete) {
            timer.color = new Color(0, 0, 0, 0);
        }
        else
        {
            timer.color = defColor;
        }
    }
}
