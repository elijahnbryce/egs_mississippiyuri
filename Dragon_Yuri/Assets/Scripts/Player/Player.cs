using Assets.Scripts.Enemy;
using UnityEngine;

public class Player : Entity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out var enemy))
        {
            TakeDamage(enemy.Dmg);
        }
    }
}
