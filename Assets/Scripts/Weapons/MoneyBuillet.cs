using UnityEngine;

public class MoneyBuillet : MonoBehaviour
{

    public float speed = 20f;
    public float lifeTime = 2f;
    public int damage = 10;  // Damage dealt to the player

    private void Start()
    {
        Destroy(gameObject, lifeTime);  // Destroy bullet after a certain time
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);  // Move bullet forward
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply damage to the player
          //  HealthManager.instance.TakeDamage(damage);  // Call TakeDamage method on HealthManager 
            Debug.Log("Bullet hit player! Player took " + damage + " damage.");
        }

    }
}
