using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private float bulletLifetime = 3f;
    public float bulletSpeed;

    [Header("Bullet Health Era")]
    public BulletType bulletType;
    public int damage = 20; 
    public bool damageEnemy, damagePlayer;
    public float speed = 20f;
    private void Start()
    {
        Destroy(gameObject, bulletLifetime);  // Destroy bullet after a certain time
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);  // Move bullet forward
    }
    private void OnCollisionEnter(Collision collision)
    {
        // If the bullet hits the player and is an enemy bullet
        if (collision.gameObject.CompareTag("Player") && bulletType == BulletType.EnemyBullet)
        {
            Debug.Log("Bullet hit player: " + collision.gameObject.name);
            PlayerHealth.instance.DamagePlayer(50);
        }
        // If the bullet hits the enemy and is a player bullet
        else if (collision.gameObject.CompareTag("Enemy") && bulletType == BulletType.PlayerBullet)
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                bool isDuck = collision.gameObject.name.Contains("Duck");
                enemyHealth.DamageEnemey(damage, isDuck);
            }
        }

        // Impact effect
        if (impactEffect != null)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(impactEffect, contact.point, Quaternion.identity);
        }

        // Bullet hole effect
        if (bulletHolePrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject bulletHole = Instantiate(bulletHolePrefab, contact.point + contact.normal * 0.05f, Quaternion.LookRotation(-contact.normal));
            bulletHole.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(bulletHole, 2f);
        }

    }

    public enum BulletType
    {
        PlayerBullet,
        EnemyBullet
    }
}
