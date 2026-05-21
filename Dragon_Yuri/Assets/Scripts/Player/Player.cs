using Assets.Scripts.Enemy;
using UnityEngine;

public class Player : Entity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //this causes an issue with IK rig transforms getting nulled
        //when the dragons are directly hit by enemies?
        //IDK - Emery <3
       /* if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponentInParent<Enemy>();
            if (null != enemy)
            {
                TakeDamage(enemy.Dmg);
            }
        }*/
    }
}
