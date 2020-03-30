using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    int currentSelection;
    public List<GameObject> selectionArray = new List<GameObject>();

    public enum Menu {Main, Difficulty}

    public GameObject mainScreen;
    public GameObject difficultyScreen;




    void Start() {
        currentSelection = 0;
        // EventSystem.current.SetSelectedGameObject(selectionArray[currentSelection],null);
        GameStats.ResetStats();
    }


    public float Difficulty 
    { 
        set
        {
            switch (value)
            {
                case 1:
                    GameStats.difficulty = GameStats.EASY;
                    GameStats.difficultyMultiplier = GameStats.EASY_MULTIPLIER;
                    break;
                case 2:
                    GameStats.difficulty = GameStats.MEDIUM;
                    GameStats.difficultyMultiplier = GameStats.MEDIUM_MULTIPLIER;
                    break;
                case 3:
                    GameStats.difficulty = GameStats.HARD;
                    GameStats.difficultyMultiplier = GameStats.HARD_MULTIPLIER;
                    break;
            }
            Debug.Log("Difficulty has been set to " + GameStats.difficulty);
        }
    }


    public void ChangeMenu(Menu menu)
    {
        switch(menu)
        {
            case Menu.Main:
                mainScreen.SetActive(true);
                difficultyScreen.SetActive(false);
                break;
            case Menu.Difficulty:
                mainScreen.SetActive(false);
                difficultyScreen.SetActive(true);
                break;
        }
    }


    public void StartGame ()
    {
        SceneManager.LoadScene("SchoolScene");
    }
    

    public void QuitGame () 
    {
        Application.Quit();
    }
}
