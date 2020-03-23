using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public Player playerRef;

    [HideInInspector] public int previousSelectedWeapon = 0;
    [HideInInspector] public int selectedWeapon = 0;

    
    void Start()
    {
        playerRef = GameManager.playerRef.GetComponent<Player>();

        SelectWeapon();
    }


    void Update()
    {
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

        if (!playerRef.isCarryingItem)
        {
            // We only want to update this if the player is not carrying an item.
            // That way, previousSelectedWeapon will equal the weapon the player 
            // had before picking something up.
            previousSelectedWeapon = selectedWeapon;

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
