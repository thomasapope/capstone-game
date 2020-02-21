using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
   public void LoadNextLevel()
   {
       // Sends back to Main Menu
		SceneManager.LoadScene("MainMenu");
   }
}
