using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroComicController : MonoBehaviour
{
    [HideInInspector] public Image image;

    public Sprite[] sprites;
    public int frame = -1;

    private const float FRAME_TIME = 4f; // Seconds per frame
    private const float TEXT_TIME = 1f; // Seconds per text field

    public float timer = 0f; // Timer for automatic frame advancement
    private float timerTarget;

    public Text[] texts;
    public Text settingText;
    public Text pressKeyText;


    public AudioSource audio;


    void Start()
    {
        image = GetComponent<Image>();
        audio = GetComponent<AudioSource>();

        if (image)
        {
            image.enabled = false;
        }

        timerTarget = FRAME_TIME;
    }


    void Update()
    {
        int previousFrame = frame;

        // Advance to next frame when button is pressed or timer expires
        if ((timer > timerTarget && frame >= 0) || Input.anyKeyDown)
        // if (Input.anyKeyDown)
        {
            frame++;
            timer = 0;
            if (frame == 0)
            {
                image.enabled = true;
                settingText.enabled = false;
                print("click");
            }
        }
        

        // If past the backstory text
        if (frame >= 0)
        {
            // If the frame has changed, update the image
            if (frame != previousFrame)
            {
                if (frame < sprites.Length) // Proceed to next frame
                {
                    image.sprite = sprites[frame];
                }
                else if (frame < sprites.Length + texts.Length + 1) // After the last frame, display the game objective
                {
                    if (frame == sprites.Length)
                    {
                        // Hide image
                        image.enabled = false;
                        timerTarget = TEXT_TIME;
                    }
                    else 
                    {
                        // Play gunshot and reveal the next line
                        audio.Play();
                        texts[frame - sprites.Length - 1].gameObject.SetActive(true);
                    }
                }
                else
                {
                    // Load the school scene
                    SceneManager.LoadScene("SchoolScene");
                }
            }

            timer += Time.deltaTime;
        }
    }
}
