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
            health = (health / maxHealth) * t.maxHealth;
            maxHealth = (int) t.maxHealth;
            speed = t.speed;
            strength = t.strength;
            defense = t.defense;
            sr.sprite = t.sprite ?? sr.sprite;
            sr.color = t.colour;
            type = t;
        }

        protected override void Start()
        {
            base.Start();
            sr = GetComponent<SpriteRenderer>();
            RotateTowardsTarget();
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                Move();
            }
        }


        private void RotateTowardsTarget()
        {
            _rb.MoveRotation(Quaternion.LookRotation(target.position));
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

        public void HitWithProjectile (Object obj) // switch with projectile class when made
        {
            if (type.critical) return; // && projectile.elements.Count() >= 2) TakeDamage(projectile.damage);
            else TakeDamage(1f); // TakeDamage(projectile.dmg, projectile.elements[0]);
        }
    }
}