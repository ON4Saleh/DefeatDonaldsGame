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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && bulletType == BulletType.EnemyBullet)
        {
            Debug.Log("Bullet hit player: " + collision.gameObject.name);
            PlayerHealth.instance.DamagePlayer(50);
        }
        else if (collision.gameObject.CompareTag("Enemy") && bulletType == BulletType.PlayerBullet)
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                bool isDuck = collision.gameObject.name.Contains("Duck"); 
                enemyHealth.DamageEnemey(damage, isDuck);
            }
        }

        //if (impactEffect != null)
        //{
        //    ContactPoint contact = collision.contacts[0];
        //    Instantiate(impactEffect, contact.point, Quaternion.identity);
        //}

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