using UnityEngine;

public class Player : Entity
{
    private void FixedUpdate()
    {
        // Rotate character
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0f) {
            _rb.angularVelocity = (-horizontal * speed * 40);
        }
    }
}
