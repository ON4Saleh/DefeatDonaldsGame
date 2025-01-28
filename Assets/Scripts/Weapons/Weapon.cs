using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float bulletLifetime = 3f;

    [SerializeField] private float shootingDelay = 0.2f;
    [SerializeField] private float burstDelay = 0.5f;
    [SerializeField] private int bulletsPerBurst = 3;
    [SerializeField] private float spreadIntensity = 0.1f;

    [SerializeField] private int maxBulletCapacity = 50;
    [SerializeField] private float reloadTime = 1f;

    [SerializeField] private Camera playerCamera;
    private Enemy enemy;
    private bool shootSound = false;
    [SerializeField] private int currentBulletCount;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool isReloading = false;
    [SerializeField] private Transform playerTransform;
    private Bandits bandits;
    internal Animator animator;

    private TextsUI textsUI;
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public bool weaponIsActive;
    private PlayerHealth playerHealth;
    public bool weaponisActive;
    private enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    [SerializeField] private ShootingMode currentShootingMode;

    private void Start()
    {
        currentBulletCount = maxBulletCapacity;
        enemy = GetComponentInParent<Enemy>();
        playerCamera = Camera.main;
        textsUI = GetComponent<TextsUI>();
        bandits = enemy.GetComponent<Bandits>();
    }

    private void Update()
    {
        if (weaponIsActive)
        {
            if (isReloading) return;

            if (Input.GetKeyDown(KeyCode.R) && currentBulletCount < maxBulletCapacity)
            {
                StartCoroutine(Reload());
                return;
            }

            HandleShooting();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleShooting()
    {
        if (!canShoot || currentBulletCount <= 0) return;

        if (gameObject.CompareTag("PlayerWeapon"))
        {
            if (currentShootingMode == ShootingMode.Single && Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(SingleFire());
                PlayShootAnimation(true);
                shootSound = true;
                SoundManager.Instance.PlaySFX("WaterGun");
            }
            else if (currentShootingMode == ShootingMode.Burst && Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(BurstFire());
                PlayShootAnimation(true);
                shootSound = true;
                SoundManager.Instance.PlaySFX("WaterGun");
            }
            else if (currentShootingMode == ShootingMode.Auto && Input.GetKey(KeyCode.Mouse0))
            {
                StartCoroutine(AutoFire());
                PlayShootAnimation(true);
                shootSound = true;
                SoundManager.Instance.PlaySFX("WaterGun");
            }
            else if (!Input.GetKey(KeyCode.Mouse0))
            {
                PlayShootAnimation(false);
                shootSound = false;
            }
        }
        else if (gameObject.CompareTag("EnemyWeapon"))
        {
            // Implement enemy shooting logic here
        }
    }

    private void PlayShootAnimation(bool isShooting)
    {
        animator.SetBool("IsShooting", isShooting);
    }

    private IEnumerator SingleFire()
    {
        canShoot = false;
        FireBullet();
        yield return new WaitForSeconds(shootingDelay);
        canShoot = true;
    }

    private IEnumerator BurstFire()
    {
        canShoot = false;
        for (int i = 0; i < bulletsPerBurst && currentBulletCount > 0; i++)
        {
            FireBullet();
            SoundManager.Instance.PlaySFX("WaterGun");
            yield return new WaitForSeconds(shootingDelay);
        }
        yield return new WaitForSeconds(burstDelay - (bulletsPerBurst * shootingDelay));
        canShoot = true;
    }

    private IEnumerator AutoFire()
    {
        canShoot = false;
        while (Input.GetKey(KeyCode.Mouse0) && currentBulletCount > 0)
        {
            FireBullet();
            SoundManager.Instance.PlaySFX("WaterGun");
            yield return new WaitForSeconds(shootingDelay);
        }
        canShoot = true;
    }

    private void FireBullet()
    {
        if (currentBulletCount <= 0) return;

        currentBulletCount--;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = playerHealth.playerDamage; // Set the bullet damage value (you can adjust this)

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterDelay(bullet, bulletLifetime));
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawnPoint.position;
        direction.Normalize();

        float spreadX = Random.Range(-spreadIntensity, spreadIntensity);
        float spreadY = Random.Range(-spreadIntensity, spreadIntensity);

        return Quaternion.Euler(spreadY, spreadX, 0) * direction;
    }

    private Vector3 EnemyCalculateDirectionAndSpread()
    {
        Vector3 direction = (playerTransform.position - bulletSpawnPoint.position).normalized; // Ensure direction is normalized
        float spreadX = Random.Range(-spreadIntensity, spreadIntensity);
        float spreadY = Random.Range(-spreadIntensity, spreadIntensity);

        return Quaternion.Euler(spreadY, spreadX, 0) * direction; // Apply spread to the direction
    }

    private void EnemyFireBullet()
    {
        if (currentBulletCount <= 0) return;

        currentBulletCount--;
        Vector3 shootingDirection = EnemyCalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        Bullet bulletScript = bullet.GetComponent<Bullet>();  // Declare bulletScript here
        if (bandits != null)
        {
            bulletScript.damage = bandits.damage;  // Set damage based on bandits
        }

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);

        Debug.Log("Bullet Fired");
        StartCoroutine(DestroyBulletAfterDelay(bullet, bulletLifetime));
    }

    public IEnumerator EnemyBurstFire()
    {
        canShoot = false;
        while (enemy.canSeePlayer()) // Assuming this method checks if the enemy can see the player
        {
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                EnemyFireBullet();
                yield return new WaitForSeconds(shootingDelay);
            }

            yield return new WaitForSeconds(burstDelay);

            yield return new WaitForSeconds(1f);
        }

        canShoot = true;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        currentBulletCount = maxBulletCapacity;
        isReloading = false;
        canShoot = true;
    }

    private IEnumerator DestroyBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
