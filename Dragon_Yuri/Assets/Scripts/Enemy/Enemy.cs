using Assets.Scripts.Enemy;
using System;
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
                if (type != value) SetType(value); 
            }
        }

        public float Dmg => strength;

        private Transform target, activeSprite = null;
        [SerializeField] private Transform spriteHolder;
        public Transform ActiveSprite
        {
            get { return activeSprite; }
            private set
            {
                if (value != ActiveSprite)
                {
                    value.gameObject.SetActive(true);
                    activeSprite?.gameObject.SetActive(false);
                    activeSprite = value;
                }
            }
        }

        protected override void Start()
        {
            base.Start();
            RotateTowardsTarget();
            spriteHolder = transform.GetChild(0);
            ActiveSprite = spriteHolder.GetChild(0);
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            Vector2 direction = (target.position - transform.position).normalized;

            Move(direction);
            RotateTowardsTarget(direction);
        }

        public void Initialize(EnemyType t) => SwitchType(t);

        public void SetTarget(Entity target) => this.target = target.transform;


        public void SwitchType(EnemyType t) => Type = t;

        private void SetType(EnemyType t)
        {
            health = (health / maxHealth) * t.maxHealth;
            maxHealth = (int)t.maxHealth;
            speed = t.speed;
            strength = t.strength;
            defense = t.defense;

            ActiveSprite = spriteHolder.GetChild(t.sprite);
            SetColour(t.colour);

            type = t;
        }

        private void SetColour(Color c)
        {
            SpriteRenderer[] sprites = new SpriteRenderer[] {
                activeSprite.GetChild(0).GetComponent<SpriteRenderer>(),
                activeSprite.GetChild(1).GetComponent<SpriteRenderer>()
            }; foreach (SpriteRenderer sr in sprites) sr.color = c;
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
            EnemyType.Element.Water => Type.wetList,
            EnemyType.Element.Fire => Type.wetList,
            _ => Type
        };

        public void HitWithProjectile(Projectile projectile)
        {
            if (type.critical) return;

            // Use first element if exists, otherwise Normal
            var element = projectile.elements.Count > 0
                ? projectile.elements[0]
                : EnemyType.Element.Normal;
        }

        private void RotateTowardsTarget()
        {
            _rb.MoveRotation(Quaternion.LookRotation(target.position));
        }
    }
}