using UnityEngine;

public class PickUp : MonoBehaviour
{
    public static PickUp instance;

    public void Awake()
    {
        if(instance != null && instance!= this)
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
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red); // Visualize ray in Scene view
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitbyRay = hit.transform.gameObject;

            if(objectHitbyRay.GetComponentInChildren<Weapon>())
{
                Debug.Log("Weapon selected");
            }

        }
    }

}
