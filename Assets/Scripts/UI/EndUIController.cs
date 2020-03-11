using UnityEngine;
using UnityEngine.UI;

public class EndUIController : MonoBehaviour
{
    public Text stats;

    public Text completionCondtion; // Victory or defeat
    public Text completionReason; // Why the player won or lost
    public Text waves; // Number of waves lasted
    public Text childrenLeft;
    public Text kills;
    public Text damage;


    void Start()
    {
        // gameObject.SetActive(false);
    }


    void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.instance.ReturnToMenu();
        }
    }


    // Updates the UI to show the correct scores. 
    // Called at the end of a level.
    public void UpdateUI()
    {
        // Completion Condition and Reason
        if (GameManager.isVictory)
        {
            // Debug.Log("YOU WON!"); 
            completionCondtion.text = "Victory";
            completionReason.text = "You got to the 'choppa";
        }
        else
        {
            // Debug.Log("YOU LOST!");
            completionCondtion.text = "Defeat";
            completionReason.text = "You were devoured by moles";
        }

        waves.text = "on the " + AddOrdinal(WaveSpawner.nextWave + 1) + " wave ";

        childrenLeft.text = "Children Left: " + GameManager.numOfChildren;

        kills.text = "Kills: " + GameManager.kills;

        damage.text = "Damage: " + GameManager.damage;
    }


    // Add ordinals to a number. Used for waves.
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
