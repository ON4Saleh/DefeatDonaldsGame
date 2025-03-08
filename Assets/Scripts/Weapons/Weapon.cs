using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float bulletLifetime = 3f;

    [SerializeField] float shootingDelay = 0.2f;
    [SerializeField] float burstDelay = 0.5f;
    [SerializeField] int bulletsPerBurst = 3;
    [SerializeField] float spreadIntensity = 0.1f;

    [SerializeField] int maxBulletCapacity = 50;
    [SerializeField] float reloadTime = 1f;

    [SerializeField] Camera playerCamera;
    private Enemy enemy;
    private bool shootsound = false;
    [SerializeField] int currentBulletCount;
    [SerializeField] bool canShoot = true;
    [SerializeField] bool isReloading = false;
    [SerializeField] private Transform playerTransform;

    internal Animator animator;

    private TextsUI textsUI;
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
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
    }

    private void Update()
    {
        if (weaponisActive)
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
                shootsound = true;
                SoundManager.Instance.PlaySFX("WaterGun");
            }
            else if (currentShootingMode == ShootingMode.Burst && Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(BurstFire());
                PlayShootAnimation(true);
                shootsound = true;
                SoundManager.Instance.PlaySFX("WaterGun");
            }
            else if (currentShootingMode == ShootingMode.Auto && Input.GetKey(KeyCode.Mouse0))
            {
                StartCoroutine(AutoFire());
                PlayShootAnimation(true);
                shootsound = true;
                SoundManager.Instance.PlaySFX("WaterGun");
            }
            else if (!Input.GetKey(KeyCode.Mouse0))
            {
                PlayShootAnimation(false);
                shootsound = false;
            }
        }
        else if (gameObject.CompareTag("EnemyWeapon"))
        {

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
        if (currentBulletCount <= 0)
        {
            return;
        }

        currentBulletCount--;
        // Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //   GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        //   bullet.transform.forward = shootingDirection;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = 20;
        Debug.Log("Bullet fired with damage: " + bulletScript.damage);

      //  Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
       // bulletRigidbody.AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
       // GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
       
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
        Vector3 direction = (playerTransform.position - bulletSpawnPoint.position).normalized; 
        float spreadX = Random.Range(-spreadIntensity, spreadIntensity);
        float spreadY = Random.Range(-spreadIntensity, spreadIntensity);

        return Quaternion.Euler(spreadY, spreadX, 0) * direction; 
    }

    private void EnemyFireBullet()
    {
        if (currentBulletCount <= 0) return;

        currentBulletCount--;
        Vector3 shootingDirection = EnemyCalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        Debug.Log("Bullet Fired");
        StartCoroutine(DestroyBulletAfterDelay(bullet, bulletLifetime));
    }
    private void OnDrawGizmos()
    {
        if (bulletSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(bulletSpawnPoint.position, bulletSpawnPoint.position + bulletSpawnPoint.forward * 10f);
        }
    }

    public IEnumerator EnemyBurstFire()
    {
        canShoot = false;
        while (enemy.canSeePlayer())
        {
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                EnemyFireBullet();
                yield return new WaitForSeconds(shootingDelay);
            }
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