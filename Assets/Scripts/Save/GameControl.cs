using UnityEngine;

using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public PlayerHealth playerHealth; 

    public void OnSaveButtonClicked()
    {
        playerHealth.SaveGame();
    }

    public void OnLoadButtonClicked()
    {
        playerHealth.LoadGame();
    }
}
