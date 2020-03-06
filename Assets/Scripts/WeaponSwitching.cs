using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    
    void Start()
    {
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


    // void SwitchWeapon(int weaponIndex)
    // {
    //     // if (weaponPoint.childCount != 0)
    //     {
    //         // Debug.Log(weaponPoint.GetChild(0).name);
    //         // Destroy(weaponPoint.GetChild(0).gameObject);

    //         // GameObject weapon = Instantiate(weapons[currentWeapon].prefab, weaponPoint.position, transform.rotation);
    //         // weapon.transform.SetParent(weaponPoint);
    //     }
    // }


    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                // Debug.Log("Weapon: " + weapon.gameObject.name + " Ref: " + GameManager.playerRef.name);
                // if (GameManager.playerRef) Debug.Log("There is a playerref");
                GameManager.playerRef.GetComponent<Player>().attackAnimator = weapon.gameObject.GetComponent<Animator>();
            }
            else 
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }

        // float w = Input.GetAxis("Mouse ScrollWheel");
        // if (w > 0f) // if scrollwheel has moved up
        // {
        //     currentWeapon = (currentWeapon + 1) % weapons.Length;
        //     weaponIsSwitching = true;
        // }
        // else if (w < 0f) // if scrollwheel has moved down
        // {
        //     currentWeapon--;
        //     if (currentWeapon < 0) currentWeapon = weapons.Length - 1;
        //     weaponIsSwitching = true;
        // }

        // if (weaponIsSwitching) 
        // {
        //     // Debug.Log("Number of weapons: " + weapons.Length);
        //     Debug.Log("Switching to: " + currentWeapon);
        //     SwitchWeapon(currentWeapon);
        //     weaponIsSwitching = false;
        // }
    }
}
