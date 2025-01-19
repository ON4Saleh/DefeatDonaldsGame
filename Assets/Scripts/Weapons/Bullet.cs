using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private GameObject bulletHolePrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Hit " + collision.gameObject.name);
        }
        if (impactEffect != null)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(impactEffect, contact.point, Quaternion.identity);
        }
        if (bulletHolePrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject bulletHole = Instantiate(bulletHolePrefab, contact.point + contact.normal * 0.05f, Quaternion.LookRotation(-contact.normal));
            bulletHole.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(bulletHole, 2f);
        }
        Destroy(gameObject);
    }
}