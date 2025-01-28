using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private Bandits bandit;
    private PlayerController playercontroller;

    [Header("Enemy UI")]
    public Image healthimg;
    public int currentHealth;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();  // Find the PlayerHealth component
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found in the scene!");
        }

        playercontroller = FindFirstObjectByType<PlayerController>();  // Find the PlayerController component
        if (playercontroller == null)
        {
            Debug.LogError("PlayerController component not found in the scene!");
        }

        bandit = GetComponent<Bandits>();  // Get the Bandits component on the same GameObject
        if (bandit != null)
        {
            currentHealth = bandit.health;
        }
        else
        {
            Debug.LogError("Bandits component not found on this GameObject!");
        }

        UpdateHealthUI();
    }

    public void TakeDamage()
    {
        currentHealth -= playerHealth.playerDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);  // Destroy the enemy when health reaches 0
        }
        UpdateHealthUI();
    }

    public void ApplyDamage()
    {
        if (gameObject.name == "Duck")
        {
            playerHealth.playerWaterLevel -= bandit.damage;  // Decrease player's water level by bandit damage
            if (playerHealth.playerWaterLevel <= 0)
            {
                playercontroller.CheckRespawn();
                playerHealth.playerWaterLevel += 1000;  // Reward player with water level
                playerHealth.UpdateScore(200);  // Increase score for killing Duck
            }
        }
        else if (gameObject.name == "Donald")
        {
            playerHealth.playerMoneyLevel -= bandit.damage;  // Decrease player's money level by bandit damage
            if (playerHealth.playerMoneyLevel <= 0)
            {
                playercontroller.CheckRespawn();
                playerHealth.playerMoneyLevel += 1000;  // Reward player with money level
                playerHealth.UpdateScore(200);  // Increase score for killing Donald
            }
        }
    }

    private void UpdateHealthUI()
    {
        float healthFraction = (float)currentHealth / Mathf.Max(bandit.health, 1f);  // Prevent division by zero
        healthimg.fillAmount = healthFraction;
    }
}
