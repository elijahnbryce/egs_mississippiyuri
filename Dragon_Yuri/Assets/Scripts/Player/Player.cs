using UnityEngine;

public class Player : Entity
{
    private void FixedUpdate()
    {
<<<<<<< HEAD
        // Rotate character
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0f) {
            _rb.angularVelocity = (-horizontal * speed * 40);
=======
        if (collision.TryGetComponent<Enemy>(out var enemy))
        {
            TakeDamage(enemy.Dmg);
>>>>>>> parent of 5e6d47c (Merge branch 'main' of https://github.com/elijahnbryce/egs_mississippiyuri)
        }
    }
}
