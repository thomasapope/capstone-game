using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager;


    void OnTriggerEnter()
    {
        // GameManager.isVictory = true;
        // gameManager.CompleteLevel();

        if (!WaveSpawner.complete)
        {
            Debug.Log("WAVES NOT COMPLETE");
        }
        else
        {
            GameManager.isVictory = true;
            GameManager.instance.EndGame();
        }
    }
}
