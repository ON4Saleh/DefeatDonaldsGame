using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerData.json");
    }

    public void SavePlayerData(PlayerHealth playerHealth)
    {
        PlayerData data = new PlayerData
        {
            position = playerHealth.transform.position,
            playerWaterLevel = playerHealth.playerWaterLevel,
            playerMoneyLevel = playerHealth.playerMoneyLevel,
            score = playerHealth.playerScore 
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Player data saved to " + saveFilePath);
    }

    public void LoadPlayerData(PlayerHealth playerHealth)
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            playerHealth.transform.position = data.position;
            playerHealth.playerWaterLevel = data.playerWaterLevel; 
            playerHealth.playerMoneyLevel = data.playerMoneyLevel; 
            playerHealth.playerScore = data.score; 
            playerHealth.UpdateHealthUI();
            playerHealth.UpdateScoreUI(); 
            Debug.Log("Player data loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + saveFilePath);
        }
    }
}