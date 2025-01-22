using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance { get; set; }
    [SerializeField] List<GameObject> weaponSlots;
    public GameObject activeweaponSlot;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void Start()
    {
        activeweaponSlot = weaponSlots[0];
        SwitchActiveSlot(0); // Ensure slot 0 is active at start
    }
    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeweaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);  // Switch to Slot 1
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);  // Switch to Slot 2
        }
    }


    public void PickUpWeapon(GameObject pickedWeapon)
    {
        // Drop the current active weapon (if any)
        dropCurrentWeapon(pickedWeapon);

        // Add the picked weapon to the active slot and set it as active
        AddWeaponIntoSlot(pickedWeapon);
    }

    private void AddWeaponIntoSlot(GameObject pickedWeapon)
    {
        // Assign the picked weapon to the active slot, not always the first slot
        // Use the current active weapon slot instead of the hardcoded weaponSlots[0]
        GameObject currentSlot = activeweaponSlot;

        // Set the picked weapon as a child of the active weapon slot
        pickedWeapon.transform.SetParent(currentSlot.transform, false);

        // Retrieve the Weapon component
        Weapon weapon = pickedWeapon.GetComponent<Weapon>();

        // Set the position and rotation based on the weapon's specified spawn position and rotation
        pickedWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        // Mark the weapon as active
        weapon.weaponisActive = true;
        weapon.animator.enabled = true;
    }


    private void dropCurrentWeapon(GameObject pickedWeapon)
    {
        // Check if the current active weapon exists
        if (activeweaponSlot.transform.childCount > 0)
        {
            // Get the current active weapon in the slot
            var weaponToDrop = activeweaponSlot.transform.GetChild(0).gameObject;

            // Mark the current weapon as inactive
            weaponToDrop.GetComponent<Weapon>().weaponisActive = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;
            // Move the current weapon to the same position as the picked weapon (for consistency, but could be customized)
            weaponToDrop.transform.SetParent(pickedWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        Debug.Log("d");
        // Deactivate the current weapon if it exists
        if (activeweaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeweaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.weaponisActive = false;
        }

        // Update the active weapon slot
        activeweaponSlot = weaponSlots[slotNumber];

        // Activate the weapon in the new slot if it exists
        if (activeweaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeweaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.weaponisActive = true;
        }
    }

}