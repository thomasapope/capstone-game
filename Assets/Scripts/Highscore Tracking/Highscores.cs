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
        // Load the highscore list from player prefs
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighscoreList list = JsonUtility.FromJson<HighscoreList>(jsonString);

        // If no list was saved in the player prefs, create one
        if (list == null)
        {
            list = new HighscoreList { scoreList = new List<HighscoreEntry>() };
        }

        return list;
    }


    // Checks if a score if high enough to make the list
    // Returns an int indicating the position that the player made or -1 if the player is not in the top ten
    public static int CheckScore(int score, List<HighscoreEntry> list)
    {
        if (list.Count == 0) return 0;
        if (list.Count == 10 && score < list[list.Count - 1].score) return -1;

        int pos = -1;

        for (int i = 0; i < list.Count; i++)
        {
            if (score > list[i].score)
            {
                pos = i;
                break;
            }

            // If we reach the end and it still hasn't swapped, the last place is where it goes
            if (i == list.Count - 1 && pos == -1)
            {
                pos = list.Count;
            }
        }

        return pos;
    }


    // Adds a score to the highscore list at position pos.
    // Should use the position returned from CheckScore.
    public static void AddScore(int pos, HighscoreEntry entry, HighscoreList list)
    {
        print("Adding score " + list.scoreList.Count + " " + pos + " " + entry.score);

        list.scoreList.Insert(pos, entry);

        // Remove the last score in the list. Only the great are to be remembered.
        if (list.scoreList.Count > 10)
        {
            list.scoreList.RemoveAt(list.scoreList.Count - 1);
        }

        SaveScores(list);
    }


    //Add ordinals to a number. Used for waves.
    public static string AddOrdinal(int num)
    {
        if( num <= 0 ) return num.ToString();

        switch(num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch(num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }
}