using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public float playerWaterLevel;
    public float playerMoneyLevel;
    public float maxWaterLevel = 10000;
    public float maxMoneyLevel = 10000; // Updated max money level
    public int respawnAttempts = 2;
    public int playerScore = 0;
    private bool isRespawning = false;
    public int playerDamage = 50;

    [Header("Player UI")]
    public Image waterLevelImg;
    public Image moneyLevelImg;
    public TextMeshProUGUI scoreText;
    private Bandits bandit;
    private PlayerController playerController;
    public static PlayerHealth instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        playerMoneyLevel = maxMoneyLevel;
        playerWaterLevel = maxWaterLevel;
        scoreText.text = "Score " + playerScore;
        UpdateScoreUI();
    }

    public void ResetHealth(Vector3 respawnPosition)
    {
        playerWaterLevel = maxWaterLevel;
        playerMoneyLevel = maxMoneyLevel;
        playerScore -= 50; // Deduct score on respawn
        transform.position = respawnPosition; // Move player to respawn position
        UpdateScoreUI();
    }


    public void UpdateHealthUI()
    {
        waterLevelImg.fillAmount = playerWaterLevel / maxWaterLevel;
        moneyLevelImg.fillAmount = playerMoneyLevel / maxMoneyLevel;
    }

   
    public void TakeDamage(Bandits bandit)
    {
        if (bandit.name == "Duck")
        {
            playerWaterLevel -= bandit.damage;
            playerController.CheckRespawn();
        }
        else if (bandit.name == "Donald")
        {
            playerMoneyLevel -= bandit.damage;
            playerController.CheckRespawn();
        }
        UpdateHealthUI();
    }
    public void ApplyDamage()
    {
        bandit.health -= playerDamage;
        RestoreHealth(10);
    }
    public void RestoreHealth(float healAmount)
    {
        playerWaterLevel += healAmount;
        playerMoneyLevel += healAmount;
        UpdateScore(200);
    }

    public void UpdateScore(int scoreChange)
    {
        playerScore += scoreChange;
        if (playerScore < 0)
        {
            playerScore = 0;
        }
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score " + playerScore;
    }
}
