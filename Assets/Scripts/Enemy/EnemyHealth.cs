using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EnemyHealth : MonoBehaviour
{
    private Bandits Duckbandit;
    private Bandits Donaldbandit;
    private PlayerController playercontroller;

    [Header("Enemy UI")]
    public Image Duckhealthimg;
    public Image DonaldhealthImg;
    public int DuckcurrentHealth;
    public int DonaldcurrenrHealth;
    public Door door;

    private JsonRead jsonRead;

    void Start()
    {
        jsonRead = FindFirstObjectByType<JsonRead>(); 
        if (jsonRead != null && jsonRead.banditlist.banditlist.Length > 0)
        {
            Duckbandit = jsonRead.banditlist.banditlist[0];
            Initialize(Duckbandit, true); 
            Donaldbandit = jsonRead.banditlist.banditlist[1];
            Initialize(Donaldbandit, false); 
        }
        else
        {
            Debug.LogError("JsonRead not found or bandits data not loaded!");
        }
    }

    public void Initialize(Bandits banditData, bool isDuck)
    {
        if (isDuck)
        {
            Duckbandit = banditData;
            DuckcurrentHealth = banditData.health;
            DuckUpdateHealthUI();  
        }
        else
        {
            Donaldbandit = banditData;
            DonaldcurrenrHealth = banditData.health;
            DonaldUpdateHealthUI(); 
        }
        Debug.Log("Initializing enemy: " + banditData.name + " with health: " + banditData.health);
    }


    public void DamageEnemey(int damage, bool isDuck)
    {
        if (isDuck)
        {
            DuckcurrentHealth -= damage;
            if (DuckcurrentHealth <= 0)
            {
                DuckcurrentHealth = 0;
                PlayerHealth.instance.UpdateScore(10);
                Destroy(gameObject);
                door.OpenDoor();
            }
            DuckUpdateHealthUI();
        }
        else if(!isDuck)
        {
            DonaldcurrenrHealth -= damage;
            if (DonaldcurrenrHealth <= 0)
            {
                DonaldcurrenrHealth = 0;
                PlayerHealth.instance.UpdateScore(10);
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
            }
            DonaldUpdateHealthUI();
        }
    }

    public void DuckUpdateHealthUI()
    {
        if (Duckhealthimg == null)
        {
            Debug.LogError("Duck health image is not assigned!");
            return;
        }
        if (Duckbandit == null)
        {
            Debug.LogError("Duckbandit data is not assigned!");
            return;
        }

        float healthFraction = (float)DuckcurrentHealth / Duckbandit.health;
        Duckhealthimg.fillAmount = healthFraction;
        Debug.Log("Duck enemy health updated: " + DuckcurrentHealth);
    }
    public void DonaldUpdateHealthUI()
    {
        if (DonaldhealthImg == null)
        {
            Debug.LogError("Health image is not assigned!");
            return;
        }

        if (Donaldbandit == null)
        {
            Debug.LogError("Donaldbandit data is not assigned!"); 
            return;
        }

        float healthFraction = (float)DonaldcurrenrHealth / Donaldbandit.health;
        DonaldhealthImg.fillAmount = healthFraction;
        Debug.Log("Donald health updated: " + DonaldcurrenrHealth);
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                bool isDuck = collision.gameObject.name.Contains("Duck"); 
                enemyHealth.DamageEnemey(20, isDuck); 
            }
            else
            {
                Debug.LogError("EnemyHealth component is missing on the enemy!");
            }
        }
    }
}
