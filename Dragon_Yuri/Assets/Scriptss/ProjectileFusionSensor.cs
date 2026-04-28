using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFusionSensor : MonoBehaviour
{
    [SerializeField] private Projectile_Z owner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryFusion(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryFusion(other);
    }

    private void TryFusion(Collider2D other)
    {
        Projectile_Z otherProjectile = other.GetComponentInParent<Projectile_Z>();

        if (otherProjectile == null)
            return;

        if (otherProjectile == owner)
            return;

        if (owner.HasFused || otherProjectile.HasFused)
            return;

        // STRICT RULE:
        // Fire ONLY fuses with Water
        // Water ONLY fuses with Fire 

        if (owner.Element == Projectile_Z.ElementType.Fire &&
            otherProjectile.Element != Projectile_Z.ElementType.Water)
            return;

        if (owner.Element == Projectile_Z.ElementType.Water &&
            otherProjectile.Element != Projectile_Z.ElementType.Fire)
            return;

        owner.TryFuseWith(otherProjectile);
    }
}