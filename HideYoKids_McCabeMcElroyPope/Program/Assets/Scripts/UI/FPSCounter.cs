using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public int avgFrameRate;
    public Text displayText;


    void Start()
    {
        displayText = GetComponent<Text>();
    }
    

    public void Update ()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        displayText.text = avgFrameRate.ToString() + " FPS";
    }
    

}
