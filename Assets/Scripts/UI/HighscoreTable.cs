using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private List<HighscoreEntry> highscoreList;
    private List<Transform> highscoreEntryTransformList;

    private const int NUM_OF_SCORES = 10;

    public float templateHeight = 80f;

    void Awake() 
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // AddHighScoreEntry(999999, "Josephy");
        // AddHighScoreEntry(888888, "Rosko");
        // AddHighScoreEntry(777777, "Mole man");
        // AddHighScoreEntry(555555, "Billy");
        // AddHighScoreEntry(666666, "Thomas");
        // AddHighScoreEntry(6, "Josh the 2nd");
        // AddHighScoreEntry(5, "Josh again");
        // AddHighScoreEntry(4, "Still Josh");
        // AddHighScoreEntry(3, "Not Josh");
        // AddHighScoreEntry(2, "Ok, it's josh");

        // string jsonString = PlayerPrefs.GetString("highscoreTable");
        // HighscoreList highscores = JsonUtility.FromJson<HighscoreList>(jsonString);

        HighscoreList highscores = Highscores.LoadScores();


        highscoreList = highscores.scoreList;
        for (int i = 0; i < highscoreList.Count; i++)
        {
            for (int j = 0; j < highscoreList.Count; j++)
            {
                if (highscoreList[j].score < highscoreList[i].score)
                {
                    HighscoreEntry t = highscoreList[i];
                    highscoreList[i] = highscoreList[j];
                    highscoreList[j] = t;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();

        foreach (HighscoreEntry entry in highscoreList)
        {
            CreateHighscoreEntry(entry, entryContainer, highscoreEntryTransformList);
        }
    }


    private void CreateHighscoreEntry(HighscoreEntry entry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        // Rank
        int rank = transformList.Count + 1;
        string rankString = AddOrdinal(rank);
        entryTransform.Find("positionText").GetComponent<Text>().text = rankString;

        // Score
        int score = entry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        // Name
        string name = entry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        entryTransform.Find("entryBackground").gameObject.SetActive(rank % 2 == 1);

        transformList.Add(entryTransform);
    }


    private void AddHighScoreEntry(int score, string name)
    {
        // Create entry
        HighscoreEntry entry = new HighscoreEntry {score = score, name = name};

        // Load highscore table
        HighscoreList highscores = Highscores.LoadScores();

        // Add entry to table
        if (highscores != null)
        {
            highscores.scoreList.Add(entry);
        }
        else 
        {
            highscores = new HighscoreList { scoreList = new List<HighscoreEntry>() };
            highscores.scoreList.Add(entry);
        }

        // Save to PlayerPrefs
        // string json = JsonUtility.ToJson(highscores);
        // PlayerPrefs.SetString("highscoreTable", json);
        // PlayerPrefs.Save(); 
        Highscores.SaveScores(highscores);
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

