using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float projectileSpeed = 10f;

    private float currentAngle;

    private void Start()
    {
        InitializePosition();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
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
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        /*Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }*/
    }
}