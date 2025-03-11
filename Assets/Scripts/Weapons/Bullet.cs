using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private float bulletLifetime = 3f;
    public float bulletSpeed = 20f;

    [Header("Bullet Health Era")]
    public BulletType bulletType;
    public int damage = 20;
    public bool damageEnemy, damagePlayer;

    private EnemyHealth enemyhealth;

    private void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
    private GameObject shooter; // ????? ?????? ???? ??? ?????

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && bulletType == BulletType.EnemyBullet)
        {
            int enemyDamage;

            if (shooter != null)
            {
                Debug.Log("Player hit by bullet from: " + shooter.name); 
                if (shooter.name == "WaterGunEnemy")
                {
                    EnemyHealth enemyHealth = shooter.GetComponentInParent<EnemyHealth>(); 
                    if (enemyHealth != null)
                    {
                        enemyDamage = enemyHealth.DuckDamage;
                        PlayerHealth.instance.DamageWaterPlayer(enemyDamage, "Duck");
                        Debug.Log("duck damage" + enemyDamage);
                    }
                    else
                    {
                        Debug.LogError("EnemyHealth not found on shooter: " + shooter.name);
                    }
                }

                else if (shooter.name == "MoneyWatergun")
                {
                    EnemyHealth enemyHealth = shooter.GetComponentInParent<EnemyHealth>(); 
                    if (enemyHealth != null)
                    {
                        enemyDamage = enemyHealth.DonaldDamage;
                        PlayerHealth.instance.DamageMoneyPlayer(enemyDamage, "Donald");
                        Debug.Log("donald damage" + enemyDamage);
                    }
                    else
                    {
                        Debug.LogError("EnemyHealth not found on shooter: " + shooter.name);
                    }
                }
            }
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
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.transform.position, Quaternion.LookRotation(transform.forward));
        }


        if (bulletHolePrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject bulletHole = Instantiate(bulletHolePrefab, contact.point + contact.normal * 0.05f, Quaternion.LookRotation(-contact.normal));
            bulletHole.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(bulletHole, 2f);
        }

        Destroy(gameObject, 0.1f); 

    }
    public enum BulletType
    {
        PlayerBullet,
        EnemyBullet
    }
}
