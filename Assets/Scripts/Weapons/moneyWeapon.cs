using UnityEngine;
using System.Collections;
public class moneyWeapon : MonoBehaviour
{
    public Transform bulletSpawner;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float timeBetweenShots = 1f; // Time between each pair of bullets

    private void Start()
    {
        StartCoroutine(FireBullets()); // Start the coroutine to fire bullets
    }

    private IEnumerator FireBullets()
    {
        while (true)
        {
            // Fire the first bullet
            FireBullet();

            // Wait for a short period before firing the second bullet
            yield return new WaitForSeconds(0.1f); // 0.1s between bullets in the pair

            // Fire the second bullet
            FireBullet();

            // Wait for the remaining time before firing the next pair of bullets
            yield return new WaitForSeconds(timeBetweenShots - 0.1f); // Adjust for the first delay
        }
    }

    private void FireBullet()
    {
        // Instantiate the bullet and set its velocity
        var bullet = Instantiate(bulletPrefab, bulletSpawner.position, bulletSpawner.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = bulletSpawner.forward * bulletSpeed;
    }
}
