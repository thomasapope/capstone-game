using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Play Music
        FindObjectOfType<AudioManager>().Play("Win");
    }
}
