using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public float playerWaterLevel;
    public float playerMoneyLevel;

    public float maxWaterLevel = 1000;
    public float maxMoneyLevel = 1000;
    public int respawnAttempts = 1;
    public int playerScore = 0;
    private bool isRespawning = false;
    private bool hasRespawned = false;
    [Header("Player UI")]
    public Image waterLevelImg;
    public Image moneyLevelImg;

    public TextMeshProUGUI scoreText;
    public static PlayerHealth instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerWaterLevel = maxWaterLevel;
        playerMoneyLevel = maxMoneyLevel;
        scoreText.text = "Score " + playerScore;
    }

    public void DamageWaterPlayer(int damage, string enemyType)
    {
        Debug.Log("Player hit! Damage: " + damage);

        if (damage <= 0)
        {
            Debug.LogError("Damage value is zero or negative. Cannot apply damage.");
            return;
        }

        playerWaterLevel -= damage;

        if (playerWaterLevel < 0)
        {
            playerWaterLevel = 0;
            HandlePlayerWatrDeath(); 
        }

        UpdateWaterHealthUI();
    }
    public void DamageMoneyPlayer(int damage, string enemyType)
    {
        Debug.Log("Player hit! Damage: " + damage);

        if (damage <= 0)
        {
            Debug.LogError("Damage value is zero or negative. Cannot apply damage.");
            return;
        }

        playerMoneyLevel -= damage;

        if (playerMoneyLevel < 0)
        {
            playerMoneyLevel = 0;
            HandlePlayerMoneyDeath();
        }

        UpdateMoneyHealthUI();
    }
    private void HandlePlayerWatrDeath()
    {
        if (!hasRespawned)
        {
            RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene("WaterGameOver");
        }
    }
    private void HandlePlayerMoneyDeath()
    {
        if (!hasRespawned)
        {
            RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene("MoneyGameOver");
        }
    }
    private void RespawnPlayer()
    {
        if (isRespawning) return;
        isRespawning = true;

        transform.position = Vector3.zero;
        playerWaterLevel = maxWaterLevel;
        UpdateWaterHealthUI();
        UpdateMoneyHealthUI();
        isRespawning = false;
        hasRespawned = true; 
    }
    public void UpdateWaterHealthUI()
    {
        float Wfraction = playerWaterLevel / maxWaterLevel;
        waterLevelImg.fillAmount = Wfraction;
    }
    public void UpdateMoneyHealthUI()
    {
        float Mfraction = playerMoneyLevel / maxMoneyLevel; 
        moneyLevelImg.fillAmount = Mfraction;
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

    public void UpdateScoreUI()
    {
        scoreText.text = "Score " + playerScore;
    }

    public void SaveGame()
    {
        SaveManager saveManager = FindFirstObjectByType<SaveManager>();
        saveManager.SavePlayerData(this);
    }

    public void LoadGame()
    {
        SaveManager saveManager = FindFirstObjectByType<SaveManager>();
        saveManager.LoadPlayerData(this);
    }
}
