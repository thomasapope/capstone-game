using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Instance of self to make the GameManager easy to access

    public EndUIController completeLevelUI;
    public GameObject gameUI;

    // Game Completion Variables
    bool gameHasEnded = false;
    public static bool isVictory = false;

    public float endScreenDelay = 3f; // Delay after end of game to show end screen

    // Object References
    public static GameObject playerRef;
    public static List<GameObject> targetRefs; // Used for AI navigation

    // Cached Prefabs
    [SerializeField]
    public GameObject childPrefab;

    // Stats and Score
    public static int kills;
    public static float damage;
    public static int numOfChildren = 3;

    // public enum CauseOfDefeat {DEAD, CHILDRENSTOLEN}
    // private reasonForDeath


    private void Awake()
    {
        instance = this;

        // Set up targetRefs
        playerRef = GameObject.FindWithTag("Player");
        targetRefs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
        targetRefs.Add(playerRef);
        // childPrefab = (GameObject)Resources.Load("ChildPrefab");

        // Hook into child messages
        Child.ChildPickedUp += OnChildPickedUp;
        Child.ChildTaken += OnChildTaken;
    }


    void OnChildPickedUp(Transform location)
    {
        Debug.Log("Child Picked Up!");
    }


    void OnChildTaken(Transform location)
    {
        Debug.Log("Child Taken!");
    }
    

    public void CompleteLevel()
    {
        if (WaveSpawner.complete)
        {

            Debug.Log("YOU WON!");
            gameUI.SetActive(false);
            completeLevelUI.gameObject.SetActive(true);
            EndGame();
        }
        else 
        {
            Debug.Log("WAVES NOT COMPLETE");
        }
    }


    public void EndGame()
    {
        Debug.Log("before if");
        if (gameHasEnded == false) 
        {
            gameHasEnded = true;
            // Debug.Log("GAME OVER");

            // if (isVictory)
            // {
            //     Debug.Log("YOU WON!"); 
            // }
            // else
            // {
            //     Debug.Log("YOU LOST!");
            // }

                Debug.Log("inside if");
            Invoke("ShowEndUI", endScreenDelay);
        }
    }


    void ShowEndUI()
    {
        Debug.Log("Hiding game UI");
        gameUI.SetActive(false);

        Debug.Log("Showing end UI");
        completeLevelUI.gameObject.SetActive(true);
        completeLevelUI.UpdateUI();
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
