using UnityEngine;

public class JsonRead : MonoBehaviour
{
    public TextAsset myJsonFile; 
    public BanditsData banditlist;
    public GameObject enemyPrefab; 

    void Start()
    {
        banditlist = JsonUtility.FromJson<BanditsData>(myJsonFile.text);

        foreach (var bandit in banditlist.banditlist)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>(); 
            enemyHealth.Initialize(bandit); 
        }
    }
}