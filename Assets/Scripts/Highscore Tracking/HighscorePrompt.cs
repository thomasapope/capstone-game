using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // nameField.Select();
        StartCoroutine(SelectInputField());
    }

    IEnumerator SelectInputField()
    {
        yield return new WaitForEndOfFrame();
        nameField.ActivateInputField();
        // nameField.Select();

        // EventSystemManager.currentSystem.SetSelectedGameObject(nameField.gameObject, null);
        // nameField.OnPointerClick (null);
        // EventSystem.current.SetSelectedGameObject(nameField.gameObject, null);
        // nameField.OnPointerClick(new PointerEventData(EventSystem.current));

    }
    

    void Update()
    {
        // nameField.ActivateInputField();
        nameField.Select();

        if (Input.GetButtonDown("Submit"))
        {
            SubmitScore();
        }
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
