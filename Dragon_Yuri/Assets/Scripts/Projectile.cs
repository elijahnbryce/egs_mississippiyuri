using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
	public class Projectile: MonoBehaviour
	{
        [Header("Elemental")]
        public List<EnemyType.Element> elements = new();

        [Header("Stats")]
        public float dmg = 1.0f;
        [SerializeField] protected float spd = 1.0f;

        protected Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy.Enemy>().HitWithProjectile(this);
            }
        }

        protected void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            _rb.linearVelocity = transform.up * spd;
        }

        private void Rotate(float rot)
        {
            _rb.MoveRotation(rot);
        }

        public void Rotate(Vector2 targ)
        {
            Vector2 direction = targ - _rb.position;
            //  subtract 90 degrees bc sprite forward is top
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Rotate(angle);
        }
    }
}