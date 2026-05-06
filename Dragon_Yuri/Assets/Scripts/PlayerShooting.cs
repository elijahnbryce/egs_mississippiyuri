using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float regenDelay = 2f; // seconds after last shot
    [SerializeField] private float regenRate = 1f;  // ammo per second

    [Header("UI")]
    [SerializeField] private Slider slider;

    private float currentAmmo = 10;
    private float lastShotTime;


    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    private void Update()
    {
        HandleAmmoRegen();
    }

    // SHOOTING

    public bool TryShoot(bool normal = true)
    {
        if (currentAmmo <= 0f)
        {
            Debug.Log("No ammo!");
            return false;
        }

        if (normal) Shoot();
        currentAmmo--;
        lastShotTime = Time.time;

        UpdateAmmoUI();
        return true;
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    // ---------------- AMMO REGEN ----------------

    private void HandleAmmoRegen()
    {
        if (currentAmmo >= maxAmmo)
            return;

        // only regen if enough time has passed since last shot
        if (Time.time < lastShotTime + regenDelay)
            return;

        currentAmmo += regenRate * Time.deltaTime;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);

        UpdateAmmoUI();
    }

    // ---------------- UI ----------------

    private void UpdateAmmoUI()
    {
        float t = (float)currentAmmo / maxAmmo;

        Debug.Assert(slider != null, $"{gameObject.name} has no slider assigned");

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = t;
        }
    }
}
