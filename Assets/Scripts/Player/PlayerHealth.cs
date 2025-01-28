using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public float playerWaterLevel;
    public float playerMoneyLevel;
    public float maxwaterlevel = 1000;
    public float maxmoneyLevel = 1000; 
    public Image waterlevelimg;
    public Image moneylevelimg;
    public int playerScore= 0;
    public TextMeshProUGUI scoreText;
    private bool isRespawning = false;
   void Start()
    {
        playerScore = 0;
        playerMoneyLevel = maxmoneyLevel;
        playerWaterLevel = maxwaterlevel;
        scoreText.text = "Score " + playerScore;
    }

    void Update()
    {
        if (!isRespawning) // Only update if not respawning
        {
            playerWaterLevel = Mathf.Clamp(playerWaterLevel, 0, maxwaterlevel);
            playerMoneyLevel = Mathf.Clamp(playerMoneyLevel, 0, maxmoneyLevel);
            UpdateHealthUI();

            if (Input.GetKeyDown(KeyCode.K))
            {
                takeDamage(Random.Range(5, 10));
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                RestoreHealth(Random.Range(5, 10));
            }
        }
    }
    public void ResetHealth()
    {
        playerWaterLevel = maxwaterlevel;
        playerMoneyLevel = maxmoneyLevel;
        playerScore -= 50; // Deduct score on respawn
        UpdateScoreUI();
    }
    public void UpdateHealthUI()
    {
        Debug.Log("WaterLevel: " + playerWaterLevel);
        Debug.Log("MoneyLevel: " + playerMoneyLevel);

        float fillW = waterlevelimg.fillAmount;
        float Wfraction = playerWaterLevel / maxwaterlevel;
        waterlevelimg.fillAmount = Wfraction; 

        float fillM = moneylevelimg.fillAmount;
        float Mfraction = playerMoneyLevel / maxmoneyLevel;
        moneylevelimg.fillAmount = Mfraction;
    }
    public void takeDamage(float damage)
    {
        playerMoneyLevel -= damage;
       // playerWaterLevel -= damage;
        UpdateScore(-300);
    }
    public void RestoreHealth(float healAmount)
    {
        playerWaterLevel += healAmount;
        playerMoneyLevel += healAmount;
        UpdateScore(5);
    }
    public void UpdateScore(int scoreChange)
    {
        playerScore += scoreChange;
        //if (playerScore < 0)
        //{
        //    playerScore = 0;
        //}
        UpdateScoreUI(); 
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score " + playerScore;
            Debug.Log("Score updated: " + playerScore); 
        }
        else
        {
            Debug.LogError("Score Text is not assigned!"); 
        }
    }
}
