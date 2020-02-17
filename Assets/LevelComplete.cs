using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
   public void LoadNextLevel()
   {
       // For now, just restart the level
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
}
