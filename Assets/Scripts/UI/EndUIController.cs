using UnityEngine;
using UnityEngine.UI;

public class EndUIController : MonoBehaviour
{
    [HideInInspector] public AudioSource audio;
    public AudioClip textSound;
    public AudioClip tallySound;
    public AudioClip returnSound;
    
    public Text completionCondtionText; // Victory or defeat
    public Text completionReasonText; // Why the player won or lost
    public Text wavesText; // Number of waves lasted
    public Text childrenSavedText;
    public Text killsText;
    public Text damageText;
    public Text scoreText;
    public Text returnText;
    public Text HighScoreText;
    public Text NewText;

    public HighscorePrompt highscorePrompt;

    private const float TEXT_DURATION = 0.75f;
    private const float DEFAULT_DURATION = 3f;

    private float timer; // Timer for score tallying
    private float duration = TEXT_DURATION; // Duration of each tally

    private enum fields { Condition, Reason, Score, Wait, Waves, ChildrenSaved, Kills, Damage, HighScore, Return }
    [SerializeField] private fields field = 0; // The field currently being tallied

    // private int waves, waveScore;
    // private int childrenSaved;
    // private int kills;
    // private int damage;
    private int scoreTally; // running score variable. used when incrementing score
    private int score;

    private bool skipped = false;
    private bool isHighScore = false;
    private int rank = -1;


    void OnEnable()
    {
        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        audio = GetComponent<AudioSource>();
        timer = 0;


        // End the game if it hasn't already
        if (!GameManager.instance.gameHasEnded)
        {
            GameManager.instance.gameHasEnded = true;
        }

        FindObjectOfType<AudioManager>().Play("EndGame");

    }


    void Update()
    {
        if (Input.GetButtonDown("Pickup"))
        {
            if (field == fields.Return) // If the score tally has been finished
            {
                GameManager.instance.ReturnToMenu();
            }
            else // Skip the tally
            {
                skipped = true;
                timer = duration + 1;
            }
        }

        // Increment timer for score tallying
        timer += Time.deltaTime;
        if( timer > duration )
        {
            // Set text fields active
            // There are two switch statements for the same condition because
            // these cases execute at the end of the timer, while the other ones 
            // need to execute throughout the timer.
            switch (field)
            {
                case fields.Condition:
                    // Completion Condition and Reason
                    if (GameManager.isVictory)
                    {
                        // Debug.Log("YOU WON!"); 
                        completionCondtionText.text = "Victory";
                        completionReasonText.text = "You got to the 'choppa";
                    }
                    else
                    {
                        // Debug.Log("YOU LOST!");
                        completionCondtionText.text = "Defeat";
                        completionReasonText.text = "You were devoured by moles";
                    }
                    ActivateField(completionCondtionText);
                    break;
                case fields.Reason:
                    ActivateField(completionReasonText);
                    break;
                case fields.Score:
                    ActivateField(scoreText);
                    break;
                case fields.Wait: // Adds a delay between score showing up and the tally starting
                    duration = 1.5f;
                    break;
                case fields.Return:
                    // ActivateField(returnText);
                    duration = 0f;
                    if (!returnText.gameObject.activeInHierarchy)
                    {
                        returnText.gameObject.SetActive(true);

                        audio.clip = returnSound;
                        audio.Play();

                        // If score is a highscore, prompt for name and save score
                        if (isHighScore)
                        {
                            Cursor.lockState = CursorLockMode.None;

                            highscorePrompt.gameObject.SetActive(true);
                            highscorePrompt.score = score;
                            highscorePrompt.pos = rank;
                        }
                    }
                    break;
            }

            // Reset timer unless skipped
            if (!skipped)
            {
                timer = 0;
            }

            if (field != fields.Return)
            {
                field++; // Move on to the next field when this one is finished
            }
        }

        // Tally the current field
        switch (field)
        {
            case fields.Waves: // Waves
                TallyField(WaveSpawner.nextWave, wavesText, "Waves Survived: ", GameStats.WAVE_SCORE);
                break;
            case fields.ChildrenSaved: // Children Saved
                TallyField(GameStats.childrenSaved, childrenSavedText, "Children Saved: ", GameStats.CHILD_SCORE);
                break;
            case fields.Kills: // Kills
                TallyField(GameManager.kills, killsText, "Kills: ", GameStats.KILL_SCORE);
                break;
            case fields.Damage: // Damage
                TallyField(GameManager.damage, damageText, "Damage Dealt: ", 1f);
                score += scoreTally;
                duration = .25f;
                break;
            case fields.HighScore: // HighScore
                HighscoreList list = Highscores.LoadScores();

                if (!HighScoreText.gameObject.activeInHierarchy)
                {
                    int pos = Highscores.CheckScore(score, list.scoreList);
                    if (pos != -1 && pos != 10)
                    {
                        // You made it to the highscore list!
                        // Highscores.AddScore(pos, entry, highscores);
                        isHighScore = true;
                        rank = pos;

                        // Play sound
                        audio.clip = textSound;
                        audio.Play();

                        // Display message
                        if (pos == 0)
                        {
                            // You got 1st place!
                            HighScoreText.text = "You got 1st place!\n" + 
                                                "You were ahead by " + list.scoreList[pos - 1].score + "points!";
                        }
                        else
                        {
                            HighScoreText.text = "You got " + Highscores.AddOrdinal(pos + 1) + " place!\n" +
                                                "The next highest score is " + list.scoreList[pos].score + ".";
                        }

                        // Set highscore text fields to active
                        NewText.gameObject.SetActive(true);
                        HighScoreText.gameObject.SetActive(true);
                    }
                    else
                    {
                        // Better luck next time.
                        print ("Score " + score + " too low. You didn't make the cut.");
                    }
                }



                // if ((score + scoreTally) > PlayerPrefs.GetInt("HighScore")) 
                // {
                //     PlayerPrefs.SetInt("HighScore", (score + scoreTally)); 
                //     NewText.gameObject.SetActive(true);
                // }
                // TallyField(PlayerPrefs.GetInt("HighScore"), HighScoreText, "HighScore: ", 0);
                break;
        }
    }


    // Sets a non tally field to active
    void ActivateField(Text field)
    {
        duration = TEXT_DURATION;
        if (!field.gameObject.activeInHierarchy)
        {
            field.gameObject.SetActive(true);

            audio.clip = textSound;
            audio.Play();
        }
    }


    // Tallies the score for a field and updates the text accordingly
    void TallyField (int target, Text field, string message, float multiplier) 
    {
        // Check if active
        if (!field.gameObject.activeInHierarchy)
        {
            score += scoreTally; // One time update of base score
            if (target > 0 && !skipped)
            {
                duration = Mathf.Min(DEFAULT_DURATION, target / 3.0f); // adjust duration to size of target
            }
            else
            {
                duration = TEXT_DURATION;
                audio.clip = tallySound;
                audio.Play();
            }
            field.gameObject.SetActive(true);
            Debug.Log("Target: " + target + " Duration: " + duration);
        }

        int tally;
        // Tally score
        tally = (int)Mathf.Ceil(((timer / duration) * target));
        scoreTally = (int)Mathf.Ceil(((timer / duration) * target * multiplier * GameStats.difficultyMultiplier));
        if (target > 0 && !skipped)
        {
            audio.clip = tallySound;
            audio.Play();
        }
        field.text = message + tally;
        // scoreTally = tally * multiplier;
        // score += scoreTally;
        scoreText.text = "Score: " + (score + scoreTally);
    }


    // Updates the UI to show the correct scores. 
    // Called at the end of a level.
    // public void UpdateUI()
    // {
    //     // waves.text = "on the " + AddOrdinal(WaveSpawner.nextWave + 1) + " wave ";

    //     childrenSavedText.text = "Children Saved: " + GameStats.childrenSaved;

    //     killsText.text = "Kills: " + GameManager.kills;

    //     damageText.text = "Damage: " + GameManager.damage;

    //     scoreText.text = "Score: " + CalculateScore();
    // }


    // public int CalculateScore()
    // {
    //     int killScore = GameManager.kills * GameStats.KILL_SCORE;
    //     int childScore = GameManager.numOfChildren * GameStats.CHILD_SCORE;
    //     int waveScore = WaveSpawner.nextWave * GameStats.WAVE_SCORE;
    //     int damageScore = GameManager.damage;

    //     return (int)((killScore + childScore + waveScore + damageScore) * GameStats.difficultyMultiplier);
    // }


    // Add ordinals to a number. Used for waves.
    // public static string AddOrdinal(int num)
    // {
    //     if( num <= 0 ) return num.ToString();

    //     switch(num % 100)
    //     {
    //         case 11:
    //         case 12:
    //         case 13:
    //             return num + "th";
    //     }

    //     switch(num % 10)
    //     {
    //         case 1:
    //             return num + "st";
    //         case 2:
    //             return num + "nd";
    //         case 3:
    //             return num + "rd";
    //         default:
    //             return num + "th";
    //     }
    // }
}
