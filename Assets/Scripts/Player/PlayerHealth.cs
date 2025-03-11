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

    public void DamagePlayer(int damage)
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
            HandlePlayerDeath(); 
        }

        UpdateHealthUI();
    }

    private void HandlePlayerDeath()
    {
        if (!hasRespawned)
        {
            RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    private void RespawnPlayer()
    {
        if (isRespawning) return;
        isRespawning = true;

        transform.position = Vector3.zero;
        playerWaterLevel = maxWaterLevel;
        UpdateHealthUI();
        isRespawning = false;
        hasRespawned = true; 
    }
    public void UpdateHealthUI()
    {
        float Wfraction = playerWaterLevel / maxWaterLevel;
        waterLevelImg.fillAmount = Wfraction;
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
