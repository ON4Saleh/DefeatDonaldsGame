using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRange = 10f;
    [SerializeField] private KeyCode pickupKey = KeyCode.F;
    [SerializeField] private KeyCode dropKey = KeyCode.G;

    [Header("Drop Settings")]
    [SerializeField] private float dropForce = 2f;
    private GameObject currentWeaponObject;

    private TextsUI textsUI;
    private Camera mainCamera;
    private readonly Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        mainCamera = Camera.main;
        textsUI = GetComponent<TextsUI>();
        if (mainCamera == null)
        {
            Debug.LogError($"{gameObject.name}: Main camera not found! PickUp system will be disabled.");
            enabled = false;
        }
    }

    private void Update()
    {
        HandleHover();
        HandlePickup();
        HandleDrop();
    }

    private void HandleHover()
    {
        if (TryGetWeaponInRange(out GameObject weaponObject))
        {
            textsUI.UpdateText("Press '" + pickupKey + "' to pick up " +
                " '" + dropKey + "' to Drop " + "'R' to Reload Weapon");
            currentWeaponObject = weaponObject;
        }
        else
        {
            textsUI.UpdateText(string.Empty);
            currentWeaponObject = null;
        }
    }

    private void HandlePickup()
    {
        if (!Input.GetKeyDown(pickupKey)) return;

        if (currentWeaponObject != null)
        {
            WeaponManager.instance.PickUpWeapon(currentWeaponObject);
            textsUI.UpdateText(string.Empty);
        }
    }

    private bool TryGetWeaponInRange(out GameObject weaponObject)
    {
        weaponObject = null;
        Ray ray = mainCamera.ViewportPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.TryGetComponent<Weapon>(out Weapon weapon) && !weapon.weaponisActive)
            {
                weaponObject = hit.collider.gameObject;
                return true;
            }
        }

        return false;
    }

    private void HandleDrop()
    {
        if (!Input.GetKeyDown(dropKey)) return;

        if (WeaponManager.instance != null &&
            WeaponManager.instance.activeweaponSlot != null &&
            WeaponManager.instance.activeweaponSlot.transform.childCount > 0)
        {
            DropCurrentWeapon();
        }
    }

    private void DropCurrentWeapon()
    {
        var weaponSlot = WeaponManager.instance.activeweaponSlot;
        var weaponToDrop = weaponSlot.transform.GetChild(0).gameObject;

        if (weaponToDrop.TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.weaponisActive = false;
            if (weapon.animator != null)
            {
                weapon.animator.enabled = false;
            }
            weaponToDrop.transform.SetParent(null);

            Vector3 dropPosition = transform.position + transform.forward * 2f;
            dropPosition.y += 1f;
            weaponToDrop.transform.position = dropPosition;
            if (weaponToDrop.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                Vector3 dropDirection = transform.forward + Vector3.up * dropForce;
                rb.AddForce(dropDirection.normalized * dropForce, ForceMode.Impulse);

                Vector3 randomRotation = new Vector3(
                    Random.Range(-90f, 90f),
                    Random.Range(-90f, 90f),
                    Random.Range(-90f, 90f)
                );
                rb.AddTorque(randomRotation, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (mainCamera != null)
        {
            Ray ray = mainCamera.ViewportPointToRay(screenCenter);
            Gizmos.DrawRay(ray.origin, ray.direction * pickupRange);
        }
    }
}