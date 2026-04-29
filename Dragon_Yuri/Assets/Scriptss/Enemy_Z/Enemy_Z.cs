using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Z : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    private int currentHealth;

    private Vector3 targetPosition = Vector3.zero; // (0,0,0)


    public void Initialize(EnemyData newData)
    {
        data = newData;
        currentHealth = data.maxHealth;
    }

    private void Start()
    {
        currentHealth = data.maxHealth;
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        transform.position += direction * data.moveSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); 
    }
    
    // Projectile Detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        Projectile_Z projectile = other.GetComponent<Projectile_Z>();

        if (projectile == null)
            return;

        TakeDamage(projectile.Damage, projectile.Element);

        Destroy(projectile.gameObject);
    }


    // Damage Handling
    public void TakeDamage(int damage, Projectile_Z.ElementType attackType)
    {
        float multiplier = 1f;



        int finalDamage = Mathf.RoundToInt(damage * multiplier);

        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    private void Die()
    {
        Destroy(gameObject);
    }


}