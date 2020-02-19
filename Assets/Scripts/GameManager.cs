using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 1f;

    public GameObject completeLevelUI;



    public void CompleteLevel()
    {
        if (WaveSpawner.complete)
        {
            Debug.Log("YOU WON!");
            completeLevelUI.SetActive(true);
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


    public static void ReturnToMenu()
    {
		SceneManager.LoadScene("MainMenu");
    }
}
