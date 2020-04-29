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

        
        // AddHighScoreEntry(3, "This really isn't josh");
        // AddHighScoreEntry(3, "Josh returns");
        // AddHighScoreEntry(1000000, "Better than \\/");

        HighscoreList highscores = Highscores.LoadScores();




        highscoreList = highscores.scoreList;
        if (highscoreList.Count == 0)
        {
            AddHighScoreEntry(25000, "President Josephy");
            AddHighScoreEntry(15000, "Rosko");
            AddHighScoreEntry(10000, "Josh");
            AddHighScoreEntry(9000, "Billy");
            AddHighScoreEntry(8000, "Mole man");
            AddHighScoreEntry(7000, "A cheater");
            AddHighScoreEntry(6000, "Josh again");
            AddHighScoreEntry(5000, "Gun is OP");
            AddHighScoreEntry(3000, "A bush");
            AddHighScoreEntry(1000, "One of the babies");
            highscores = Highscores.LoadScores();
            highscoreList = highscores.scoreList;
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
        string rankString = Highscores.AddOrdinal(rank);
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
        HighscoreEntry entry = new HighscoreEntry(score, name);

        // Load highscore table
        HighscoreList highscores = Highscores.LoadScores();
        
        int pos = Highscores.CheckScore(score, highscores.scoreList);
        if (pos != -1 && pos != 10)
        {
            Highscores.AddScore(pos, entry, highscores);
        }
        else
        {
            print ("Score " + score + " too low. Discarding.");
        }
        
        Highscores.SaveScores(highscores);
    }
}