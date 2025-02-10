using UnityEngine;

public class JsonRead : MonoBehaviour
{
    public TextAsset myJsonFile; 
    public BanditsData banditlist; 
    public GameObject enemyPrefab;  

    void Start()
    {
        banditlist = JsonUtility.FromJson<BanditsData>(myJsonFile.text);

        if (banditlist != null && banditlist.banditlist.Length > 0)
        {
            for (int i = 0; i < banditlist.banditlist.Length; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    bool isDuck = i == 0; 

                    enemyHealth.Initialize(banditlist.banditlist[i], isDuck);
                }
                else
                {
                    Debug.LogError("EnemyHealth component is missing on the enemy prefab!");
                }
            }
        }
        else
        {
            Debug.LogError("Bandits data not loaded correctly from JSON!");
        }
    }
}
