using UnityEngine;

public class Projectile_Z : MonoBehaviour
{
    public enum ElementType
    {
        Fire,
        Water,
        Ultra
    }

    [Header("Stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 10;

    [SerializeField] private ElementType element;

    [Header("Fusion")]
    [SerializeField] private GameObject ultraProjectilePrefab;

    private Rigidbody2D rb;
    private bool hasFused;

    public ElementType Element => element;
    public bool HasFused => hasFused;
    public int Damage => damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
        rb.linearVelocity = transform.right * speed;
        rb.linearVelocity = transform.right * speed;
    }

    public void TryFuseWith(Projectile_Z other)
    {
        if (hasFused || other.hasFused)
            return;

        if (other.Element == element)
            return;

        hasFused = true;
        other.hasFused = true;

        Vector3 spawnPos = (transform.position + other.transform.position) * 0.5f;

        if (ultraProjectilePrefab != null)
        {
            Instantiate(ultraProjectilePrefab, spawnPos, Quaternion.identity);
        }

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}