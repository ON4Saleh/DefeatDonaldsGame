using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Bandits bandit;
    private PlayerController playercontroller;

    [Header("Enemy UI")]
    public Image healthimg;
    public int currentHealth;

    public Door door;

    public void Initialize(Bandits banditData)
    {
        bandit = banditData;
        currentHealth = bandit.health;
        UpdateHealthUI();
    }

    public void DamageEnemey(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerHealth.instance.UpdateScore(10); 
            Destroy(gameObject); 
            OpenDoor(); 
        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float healthFraction = (float)currentHealth / bandit.health;
        healthimg.fillAmount = healthFraction;
        Debug.Log("Enemy health updated: " + currentHealth);
    }

    private void OpenDoor()
    {
        if (door != null)
        {
            door.OpenDoor();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth.instance.DamagePlayer(bandit.damage);
        }
    }
}
