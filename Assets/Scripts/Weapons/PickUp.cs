using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public static PickUp instance { get; set; }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red); 

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.GetComponent<Weapon>() && objectHit.GetComponent<Weapon>().weaponisActive == false)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.instance.PickUpWeapon(objectHit.gameObject); 
                }
            }
        }
    }

}
