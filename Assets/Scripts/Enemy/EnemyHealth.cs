using UnityEngine;
using UnityEngine.UI;

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
        jsonRead = FindObjectOfType<JsonRead>(); // العثور على سكربت JsonRead في المشهد

        if (jsonRead != null && jsonRead.banditlist.banditlist.Length > 0)
        {
            Duckbandit = jsonRead.banditlist.banditlist[0];
            Initialize(Duckbandit, true);  // تهيئة البط
            Donaldbandit = jsonRead.banditlist.banditlist[1];
            Initialize(Donaldbandit, false);  // تهيئة دونالد
        }
        else
        {
            Debug.LogError("JsonRead not found or bandits data not loaded!");
        }
    }

    // تهيئة البيانات للأعداء
    // دالة Initialize
    public void Initialize(Bandits banditData, bool isDuck)
    {
        if (isDuck)
        {
            Duckbandit = banditData;
            DuckcurrentHealth = banditData.health;
            DuckUpdateHealthUI();  // تحديث واجهة المستخدم للبطة
        }
        else
        {
            Donaldbandit = banditData;
            DonaldcurrenrHealth = banditData.health;
            DonaldUpdateHealthUI();  // تحديث واجهة المستخدم لDonald
        }
        Debug.Log("Initializing enemy: " + banditData.name + " with health: " + banditData.health);
    }


    // التعامل مع الضرر الذي يتعرض له العدو
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
                OpenDoor();
            }
            DuckUpdateHealthUI();
        }
        else
        {
            DonaldcurrenrHealth -= damage;
            if (DonaldcurrenrHealth <= 0)
            {
                DonaldcurrenrHealth = 0;
                PlayerHealth.instance.UpdateScore(10);
                Destroy(gameObject);
                OpenDoor();
            }
            DonaldUpdateHealthUI();
        }
    }

    // تحديث واجهة المستخدم الخاصة بالبط
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

    // تحديث واجهة المستخدم الخاصة بدونالد
    public void DonaldUpdateHealthUI()
    {
        if (DonaldhealthImg == null)
        {
            Debug.LogError("Health image is not assigned!");
            return;
        }

        if (Donaldbandit == null)
        {
            Debug.LogError("Donaldbandit data is not assigned!");  // اضافة التحقق هنا
            return;
        }

        float healthFraction = (float)DonaldcurrenrHealth / Donaldbandit.health;
        DonaldhealthImg.fillAmount = healthFraction;
        Debug.Log("Donald health updated: " + DonaldcurrenrHealth);
    }


    // فتح الباب بعد تدمير العدو
    private void OpenDoor()
    {
        if (door != null)
        {
            door.OpenDoor();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // تحديد من هو العدو الذي تلقى الضرر
                bool isDuck = collision.gameObject.name.Contains("Duck"); // افترض أن اسم الكائن يحتوي على "Duck" للعدو البط
                enemyHealth.DamageEnemey(20, isDuck); // تمرير معلومات الطلقات
            }
            else
            {
                Debug.LogError("EnemyHealth component is missing on the enemy!");
            }
        }
    }
}
