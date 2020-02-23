using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 3f;

    public GameObject completeLevelUI;
    public GameObject gameUI;
    

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
