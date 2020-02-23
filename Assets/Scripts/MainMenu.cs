using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    int currentSelection;
    public List<GameObject> selectionArray = new List<GameObject>();
    void Start() {
        currentSelection = 0;
        EventSystem.current.SetSelectedGameObject(selectionArray[currentSelection],null);
    }

    public void StartGame ()
    {
        SceneManager.LoadScene("ThomasScene");
    }

    public void QuitGame () 
    {
        Application.Quit();
    }
}
