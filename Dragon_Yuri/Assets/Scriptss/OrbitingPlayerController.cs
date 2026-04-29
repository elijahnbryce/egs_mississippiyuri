using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OrbitingPlayerController : MonoBehaviour
{
    public enum ElementType
    {
        Fire,
        Water
    }

    [Header("Element")]
    [SerializeField] private ElementType element;

    [Header("Orbit Settings")]
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float orbitRadius = 5f;
    [SerializeField] private float rotationSpeed = 180f;

    [Tooltip("Starting angle offset so players don't overlap (e.g. 0 and 180)")]
    [SerializeField] private float startAngleOffset = 0f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float regenDelay = 2f; // seconds after last shot
    [SerializeField] private float regenRate = 1f;  // ammo per second

    [Header("UI")]
    [SerializeField] private Slider leftSlider;
    [SerializeField] private Slider rightSlider;

    private float currentAngle;
    private float currentAmmo = 10;
    private float lastShotTime;


    private void Start()
    {
        currentAmmo = maxAmmo;
        InitializePosition();
        UpdateAmmoUI();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleAmmoRegen();
    }

    // Start Position Fix
    private void InitializePosition()
    {
        currentAngle = startAngleOffset;

        ApplyOrbitPositionAndRotation();
    }

    // Mvement

    private void HandleMovement()
    {
        float input = GetInput();

        currentAngle += input * rotationSpeed * Time.deltaTime;

        ApplyOrbitPositionAndRotation();
    }


    // Input per dragon type

    private float GetInput()
    {
        if (Keyboard.current == null)
            return 0f;

        switch (element)
        {
            case ElementType.Water:
                // ONLY WASD controls
                if (Keyboard.current.aKey.isPressed)
                    return 1f;

                if (Keyboard.current.dKey.isPressed)
                    return -1f;

                return 0f;

            case ElementType.Fire:
                // ONLY arrow controls
                if (Keyboard.current.leftArrowKey.isPressed)
                    return 1f;

                if (Keyboard.current.rightArrowKey.isPressed)
                    return -1f;

                return 0f;
        }

        return 0f;
    }

    // APPLY ORBIT POSITION

    private void ApplyOrbitPositionAndRotation()
    {
        float rad = currentAngle * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;

        transform.position = orbitCenter.position + (Vector3)offset;

        Vector2 outward = ((Vector2)transform.position - (Vector2)orbitCenter.position).normalized;

        float angle = Mathf.Atan2(outward.y, outward.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    // SHOOTING

    private void HandleShooting()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (currentAmmo <= 0f)
        {
            Debug.Log("No ammo!");
            return;
        }

        Shoot();
        currentAmmo--;
        lastShotTime = Time.time;

        UpdateAmmoUI();
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

        // Left slider fills from left --> center
        if (leftSlider != null)
        {
            leftSlider.minValue = 0f;
            leftSlider.maxValue = 1f;
            leftSlider.value = t;
        }

        // Right slider fills from right --> center
        if (rightSlider != null)
        {
            rightSlider.minValue = 0f;
            rightSlider.maxValue = 1f;
            rightSlider.value = t;
        }
    }
}
