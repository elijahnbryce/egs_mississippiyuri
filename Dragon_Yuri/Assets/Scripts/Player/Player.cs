using UnityEngine;

public class Player : Entity
{
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        _rb.angularVelocity = -horizontal * speed;
    }
}
