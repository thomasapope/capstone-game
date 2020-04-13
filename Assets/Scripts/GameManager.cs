using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Instance of self to make the GameManager easy to access

    public EndUIController completeLevelUI;
    public GameObject gameUI;

    // public Transform[] itemSpawnPoints;
    public List<Transform> itemSpawnPoints;
    public List<Transform> childrenSpawnPoints;

    // Game Completion Variables
    bool gameHasEnded = false;
    public static bool isVictory = false;
    public int numOfParts = 3; // Number of parts that need to be found

    public float endScreenDelay = 3f; // Delay after end of game to show end screen

    // Object References
    // public Transform playerSpawnPoint;
    public static GameObject playerRef;
    public static List<GameObject> targetRefs; // Used for AI navigation
    public Transform escapeVehicleRef;

    // Cached Prefabs
    public GameObject childPrefab;
    public GameObject[] items;


    // Stats and Score
    public static int kills;
    public static int damage;
    public static int numOfChildren = 3; // number of remaining children

    // public enum CauseOfDefeat {DEAD, CHILDRENSTOLEN}
    // private reasonForDeath


    private void Awake()
    {
        instance = this;

        // Spawn items and children
        SpawnItems();
        SpawnChildren();

        // Set up targetRefs
        playerRef = GameObject.FindWithTag("Player");
        targetRefs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
        numOfChildren = targetRefs.Count;
        foreach (GameObject child in targetRefs)
        {
            child.GetComponentInParent<Child>().ChildTaken += OnChildTaken;
        }
        targetRefs.Add(playerRef);

        escapeVehicleRef = GameObject.FindWithTag("vehicle").transform;

        // Hook into child messages
        Child.ChildPickedUp += OnChildPickedUp;
    }

    void Start()
    {
        // Play Music
        FindObjectOfType<AudioManager>().Play("School");
    }


    void SpawnItems()
    {
        foreach (GameObject item in items)
        {
            Transform _sp = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count)];

            // Transform rand = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Length)].transform;
            GameObject go = Instantiate(item);
            go.transform.position = _sp.position;
            itemSpawnPoints.Remove(_sp);
            // print(go.transform.position);
        }
    }


    void SpawnChildren()
    {
        for (int i = 0; i < GameStats.totalChildren; i++)
        {
            Transform _sp = childrenSpawnPoints[Random.Range(0, childrenSpawnPoints.Count)];

            // Transform rand = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Length)].transform;
            GameObject go = Instantiate(childPrefab);
            go.transform.position = _sp.position;
            childrenSpawnPoints.Remove(_sp);
            // print(go.transform.position);
        }
    }


    void OnChildPickedUp(Transform location)
    {
        // Debug.Log("Child Picked Up!");
    }


    void OnChildTaken()
    {
        // Debug.Log("Child Taken!");
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
        if (gameHasEnded == false) 
        {
            gameHasEnded = true;

            Invoke("ShowEndUI", endScreenDelay);
        }
    }


    void ShowEndUI()
    {
        // Debug.Log("Hiding game UI");
        gameUI.SetActive(false);

        // Debug.Log("Showing end UI");
        completeLevelUI.gameObject.SetActive(true);
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
