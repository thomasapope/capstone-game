using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscorePrompt : MonoBehaviour
{
    public int pos;
    public int score;

    public TMPro.TMP_InputField nameField;


    private void Start()
    {
        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        nameField.Select();
    }

    public void SubmitScore()
    {
        if (nameField.text != "")
        {
            HighscoreEntry entry = new HighscoreEntry(score, nameField.text);
            Highscores.AddScore(pos, entry, Highscores.LoadScores());
            gameObject.SetActive(false);
        }
    }
}
