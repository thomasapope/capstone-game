using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores : MonoBehaviour
{

    // Saves the highscores to PlayerPrefs serialized in json format
    public static void SaveScores(HighscoreList scores)
    {
        string json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save(); 
    }


    // Returns highscore table in a HighscoreList object
    public static HighscoreList LoadScores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        return JsonUtility.FromJson<HighscoreList>(jsonString);
    }
}