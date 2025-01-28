using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private int duckwaterlevel;
    private int donaldMoneyLevel;
    private Bandits bandit; // ???? ?????? ?? ???? ??? JSON
    public Image waterlevelimg; // ??? ??????? ?? Inspector
    public Image moneylevelimg; // ??? ??????? ?? Inspector

    void Start()
    {
        JsonRead jsonRead = FindObjectOfType<JsonRead>(); // ???? ??? ???? ??? JsonRead ?? ??????
        if (jsonRead == null)
        {
            Debug.LogError("JsonRead component not found!");
            return;
        }

        // ????? ??? bandit ????? ??? ??? ??????
        if (gameObject.name == "Duck")
        {
            bandit = jsonRead.banditlist.banditlist[0]; // ????? ????? "Duck"
            duckwaterlevel = bandit.maxWaterLevel;
            donaldMoneyLevel = 0; // ????? ????? ????? ?? Donald ??? 0
        }
        else if (gameObject.name == "Donald")
        {
            bandit = jsonRead.banditlist.banditlist[1]; // ????? ????? "Donald"
            donaldMoneyLevel = bandit.maxMoneyLevel;
            duckwaterlevel = 0; // ????? ????? ?????? ?? Duck ??? 0
        }
    }

    void Update()
    {
        // ?????? ?? ?? ????? ????? ?? ??? ?? 0
        if (bandit.name == "Duck")
        {
            duckwaterlevel = Mathf.Clamp(duckwaterlevel, 0, bandit.maxWaterLevel);
        }
        else if (bandit.name == "Donald")
        {
            donaldMoneyLevel = Mathf.Clamp(donaldMoneyLevel, 0, bandit.maxMoneyLevel);
        }

        UpdateHealthUI(); // ????? ????? ????????

        // ?????? ??????? ???? ????? ???????? ?????
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Check if the current GameObject is Duck or Donald
            if (bandit.name == "Duck")
            {
                takeDamage(Random.Range(5, 10));
            }
            else if (bandit.name == "Donald")
            {
                takeDamage(Random.Range(5, 10));
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Check if the current GameObject is Duck or Donald
            if (bandit.name == "Duck")
            {
                RestoreHealth(Random.Range(5, 10));
            }
            else if (bandit.name == "Donald")
            {
                RestoreHealth(Random.Range(5, 10));
            }
        }
    }

    public void ResetHealth()
    {
        duckwaterlevel = bandit.maxWaterLevel;
        donaldMoneyLevel = bandit.maxMoneyLevel;
    }
    public void UpdateHealthUI()
    {
        // Log the current health levels for debugging
        Debug.Log("WaterLevel: " + duckwaterlevel);
        Debug.Log("MoneyLevel: " + donaldMoneyLevel);

        // Update the UI images based on current health levels
        float Wfraction = (float)duckwaterlevel / bandit.maxWaterLevel;
        waterlevelimg.fillAmount = Wfraction;

        float Mfraction = (float)donaldMoneyLevel / bandit.maxMoneyLevel;
        moneylevelimg.fillAmount = Mfraction;

        // Additional logging to confirm UI updates
        Debug.Log("WaterLevel UI Fill Amount: " + waterlevelimg.fillAmount);
        Debug.Log("MoneyLevel UI Fill Amount: " + moneylevelimg.fillAmount);
    }

    public void takeDamage(int damage)
    {
        if (bandit.name == "Duck")
        {
            duckwaterlevel -= damage;
        }
        if (bandit.name == "Donald")
        {
            donaldMoneyLevel -= damage;
        }
    }

    public void RestoreHealth(int healAmount)
    {
        if (bandit.name == "Duck")
        {
            duckwaterlevel = Mathf.Clamp(duckwaterlevel + healAmount, 0, bandit.maxWaterLevel);
        }
        if (bandit.name == "Donald")
        {
            donaldMoneyLevel = Mathf.Clamp(donaldMoneyLevel + healAmount, 0, bandit.maxMoneyLevel);
        }
    }
}