using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SwitchActiveSlot(0); 
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
            SwitchActiveSlot(0); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);  
        }
    }

    public void PickUpWeapon(GameObject pickedWeapon)
    {
        dropCurrentWeapon(pickedWeapon);
        AddWeaponIntoSlot(pickedWeapon);
    }

    private void AddWeaponIntoSlot(GameObject pickedWeapon)
    {
        GameObject currentSlot = activeweaponSlot;
        pickedWeapon.transform.SetParent(currentSlot.transform, false);
        Weapon weapon = pickedWeapon.GetComponent<Weapon>();
        pickedWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
        weapon.weaponisActive = true;  // ???? ?? ?? ?????? ?????? ???
        weapon.animator.enabled = true;
    }


    private void dropCurrentWeapon(GameObject pickedWeapon)
    {
        if (activeweaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeweaponSlot.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<Weapon>().weaponisActive = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;
            weaponToDrop.transform.SetParent(pickedWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeweaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeweaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.weaponisActive = false;
            Debug.Log("Deactivating current weapon");
        }

        activeweaponSlot = weaponSlots[slotNumber];

        if (activeweaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeweaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.weaponisActive = true;
            Debug.Log("Activating new weapon");
        }
    }



}
