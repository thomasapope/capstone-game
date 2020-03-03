using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 3f;

    public GameObject completeLevelUI;
    public GameObject gameUI;

    // public static GameObject[] playerRef;
    public static GameObject playerRef;
    public static List<GameObject> targetRefs;

    public static GameObject childPrefab;

    // public static enum AIState {}

    public static int numOfChildren = 3;

    private void Start()
    {
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

            //Returns user to Main Menu
            Invoke("ReturnToMenu", restartDelay);
        }
    }


    public void ReturnToMenu()
    {
		SceneManager.LoadScene("Menu");
    }
}
