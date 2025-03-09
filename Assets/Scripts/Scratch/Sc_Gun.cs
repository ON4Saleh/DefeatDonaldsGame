using UnityEngine;
using System.Collections;

public class Sc_Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public int poolSize = 10;
    private GameObject[] bulletPool;

    void Start()
    {
        bulletPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, firePoint);
            bulletPool[i].SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        GameObject bullet = GetBullet();
        if (bullet != null)
        {
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = firePoint.forward * bulletSpeed;
            }

            StartCoroutine(DisableBullet(bullet, 2f));
        }
    }

    IEnumerator DisableBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

    GameObject GetBullet()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                bulletPool[i].SetActive(true);
                return bulletPool[i];
            }
        }
        return null;
    }
}
