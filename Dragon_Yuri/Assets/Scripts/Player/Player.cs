using Assets.Scripts.Enemy;
using UnityEngine;

public class Player : Entity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponentInParent<Enemy>();
            if (null != enemy)
            {
                TakeDamage(enemy.Dmg);
            }
        }
    }
}
