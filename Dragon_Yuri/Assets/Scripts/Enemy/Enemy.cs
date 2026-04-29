using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Enemy : Entity
    {
        private EnemyType type;
        public EnemyType Type
        {
            get { return type; }
            private set
            {
                if (type != value)
                {
                    SetType(value);
                }
            }
        }

        private Transform target;
        private SpriteRenderer sr;
        public void SetTarget(Entity target) => this.target = target.transform;

        public void SwitchType(EnemyType t) => Type = t;

        private void SetType(EnemyType t)
        {
            // first time initialization
            if (type == null)
            {
                maxHealth = (int)t.maxHealth;
                health = maxHealth; //full hp
            }
            else
            {
                // Preserve % health on type switch
                health = (health / maxHealth) * t.maxHealth;
                maxHealth = (int)t.maxHealth;
            }

            speed = t.speed;
            strength = t.strength;
            defense = Mathf.Max(t.defense, 0.0001f); // safety

            sr.sprite = t.sprite ?? sr.sprite;
            sr.color = t.colour;

            type = t;

            Debug.Log($"Initialized Enemy: HP={health}/{maxHealth}");
        }

        protected override void Start()
        {
            base.Start();
            sr = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            Vector2 direction = (target.position - transform.position).normalized;

            Move(direction);
            RotateTowardsTarget(direction);
        }


        private void RotateTowardsTarget(Vector2 direction)
        {
            if (direction == Vector2.zero) return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rb.MoveRotation(angle - 90f);
        }

        protected void TakeDamage(float damage, EnemyType.Element element)
        {
            /* Apply type weaknesses to damage before deducting health */

            // Multiplier is 2 to the power of element count
            int advantageCount = type.advantageList.Count(e => e == element);
            int weaknessCount = type.weaknessList.Count(e => e == element);

            damage *= Mathf.Pow(2, advantageCount);
            damage /= Mathf.Pow(2, weaknessCount);

            TakeDamage(damage);
        }

        protected void InteractWithElement(EnemyType.Element element) => Type = element switch {
            EnemyType.Element.Water => type.wetList,
            EnemyType.Element.Fire => type.wetList,
            _ => Type
        };

        public void HitWithProjectile(Object obj)
        {
            if (type.critical) return;

            var projectile = obj as Assets.Scripts.Projectile;

            if (projectile == null)
            {
                Debug.LogWarning("Projectile cast failed");
                return;
            }

            // Use first element if exists, otherwise Normal
            var element = projectile.elements.Count > 0
                ? projectile.elements[0]
                : EnemyType.Element.Normal;

            TakeDamage(projectile.dmg, element);

            Debug.Log($"Hit! Damage: {projectile.dmg}");
        }
    }
}