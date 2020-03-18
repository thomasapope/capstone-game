﻿using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public Player playerRef;

    public int selectedWeapon = 0;

    
    void Start()
    {
        playerRef = GameManager.playerRef.GetComponent<Player>();

        SelectWeapon();
    }


    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else 
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else 
                selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }


    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                playerRef.currentWeapon = weapon.GetComponent<Weapon>();
                // playerRef.attackAnimator = weapon.gameObject.GetComponent<Animator>();
            }
            else 
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
