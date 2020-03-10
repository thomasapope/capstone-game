using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    bool gameHasEnded = false;
    public static bool isVictory = false;

    public float endScreenDelay = 3f;
    public float returnToMenuDelay = 10f;

    public GameObject completeLevelUI;
    public GameObject gameUI;

    // public static GameObject[] playerRef;
    public static GameObject playerRef;
    public static List<GameObject> targetRefs;

    public static GameObject childPrefab;

    // public static enum AIState {}

    // Stats and Score
    public static int kills;
    public static float damage;
    public static int numOfChildren = 3;


    private void Awake()
    {
        instance = this;

        // playerRef = GameObject.FindGameObjectsWithTag("Player").ToList();
        playerRef = GameObject.FindWithTag("Player");
        targetRefs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
        targetRefs.Add(playerRef);
        childPrefab = (GameObject)Resources.Load("ChildPrefab");

        // foreach (GameObject o in targetRefs)
        // {
        //     Debug.Log(o.name);
        // }
    }
    

    public void CompleteLevel()
    {
        if (WaveSpawner.complete)
        {

            Debug.Log("YOU WON!");
            gameUI.SetActive(false);
            completeLevelUI.SetActive(true);
            EndGame();
        }
        else 
        {
            Debug.Log("WAVES NOT COMPLETE");
        }
    }


    public void EndGame()
    {
        if (gameHasEnded == false) 
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER");

            if (isVictory)
            {
                Debug.Log("YOU WON!");
                Invoke("ShowEndUI", endScreenDelay);  
            }
            else
            {
                Debug.Log("YOU LOST!");
                Invoke("ShowEndUI", endScreenDelay);
            }

            //Returns user to Main Menu
            Invoke("ReturnToMenu", returnToMenuDelay);
        }
    }


    void ShowEndUI()
    {
        gameUI.SetActive(false);
        completeLevelUI.SetActive(true);
    }


    public void ReturnToMenu()
    {
		SceneManager.LoadScene("Menu");
    }


    public static int NumOfChildren
    {
        get { return numOfChildren; }
        set 
        { 
            numOfChildren = value;
            if (numOfChildren <= 0) // No children left
            {
                GameManager.instance.EndGame();
            }
        }
    }
}
