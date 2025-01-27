using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private float bulletLifetime = 3f;

    [Header("Bullet health era")]
    public BulletType bulletType;
    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandleBulletHit(collision.gameObject, "Player");
            Debug.Log("hit player" + collision.gameObject.name + "!"); 
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleBulletHit(collision.gameObject, "Enemy");
            Debug.Log("hit enemy" + collision.gameObject.name + "!");
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

    private void HandleBulletHit(GameObject target, string targetTag)
    {
        if (targetTag == "Player")
        {
            HandlePlayerBulletHit(target);
        }
        else if (targetTag == "Enemy")
        {
            HandleEnemyBulletHit(target);
        }
    }
    private void HandlePlayerBulletHit(GameObject enemy)
    {
        Enemy enemyStats = enemy.GetComponent<Enemy>();
        if (enemyStats != null && enemy.name == "Duck")
        {
            enemyStats.enemyWaterLevel -= 30;
            Debug.Log("Duck water level: " + enemyStats.enemyWaterLevel); 
            PlayerController playerController = GameManager.Instance.playerStats;
            playerController.waterLevel -= 10;
            Debug.Log("Player water level: " + playerController.waterLevel); 
            playerController.score += 20;

            if (enemyStats.enemyWaterLevel <= 0)
            {
                Destroy(enemy);
                playerController.waterLevel += 1000;
                Debug.Log("Duck destroyed!");
            }
        }
    }
    private void HandleEnemyBulletHit(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.waterLevel -= damage;
            Debug.Log("Player water level after hit: " + playerController.waterLevel);
            playerController.score -= 50;

            if (playerController.waterLevel <= 0)
            {
                RespawnPlayer(playerController);
            }
        }
    }
    private void RespawnPlayer(PlayerController playerController)
    {
        playerController.RespawnPlayer();
    }
    public enum BulletType
    {
        PlayerBullet,
        EnemyBullet
    }
}