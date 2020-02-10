using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("DefaultState"))
        {
            Debug.Log("Swing!");
            anim.SetTrigger("swing");
        }
    }
}
