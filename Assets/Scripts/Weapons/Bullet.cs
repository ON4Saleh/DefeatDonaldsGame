using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private float bulletLifetime = 3f;
    public float bulletSpeed;
    [Header("Bullet health era")]
    public BulletType bulletType;
    public int damage;
    public bool damageEnemy, damagePlayer;
    private PlayerHealth playerhealth;
    private EnemyHealth enemyhealth;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && bulletType == BulletType.EnemyBullet)
        {
            // If it's an enemy bullet and hits the player
            Debug.Log("Enemy Bullet hit Player");
            PlayerHealth.instance.ApplyDamage();  // Apply damage to the player
        }
        else if (collision.gameObject.CompareTag("Enemy") && bulletType == BulletType.PlayerBullet)
        {
            // If it's a player bullet and hits an enemy
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("Player Bullet hit Enemy");
                enemyHealth.ApplyDamage();  // Apply damage to the enemy
            }
        }
        if (impactEffect != null)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(impactEffect, contact.point, Quaternion.identity);
        }
        if (bulletHolePrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject bulletHole = Instantiate(bulletHolePrefab, contact.point + contact.normal * 0.05f, Quaternion.LookRotation(-contact.normal));
            bulletHole.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(bulletHole, 2f);
        }
        gameObject.SetActive(false);
    }

    public enum BulletType
    {
        PlayerBullet,
        EnemyBullet
    }
}

