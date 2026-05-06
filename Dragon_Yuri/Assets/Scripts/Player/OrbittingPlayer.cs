using Assets.Scripts.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;
using Element = Assets.Scripts.Enemy.EnemyType.Element;

[RequireComponent(typeof(PlayerShooting))]
public class OrbittingPlayer : Player
{
    [Header("Element")]
    [SerializeField] private Element element;

    [Header("Orbit Settings")]
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float orbitRadius = 5f;
    [SerializeField] private float rotationSpeed = 180f;

    [Tooltip("Starting angle offset so players don't overlap (e.g. 0 and 180)")]
    [SerializeField] private float startAngleOffset = 0f;

    [Header("Shooting Settings")]
    [SerializeField] private Transform firePoint;
    [HideInInspector] public Transform FirePoint => firePoint;
    [SerializeField] private PlayerShooting psh;
    public PlayerShooting Psh => psh;

    private float currentAngle;


    protected override void Start()
    {
        base.Start();
        InitializePosition();
        psh = GetComponent<PlayerShooting>();
    }

    // Start Position Fix
    private void InitializePosition()
    {
        currentAngle = startAngleOffset;

        ApplyOrbitPositionAndRotation();
    }

    // Mvement

    public void HandleMovement(float input)
    {
        currentAngle += input * rotationSpeed * Time.deltaTime;

        ApplyOrbitPositionAndRotation();
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
}
