using Assets.Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        [SerializeField] private HealthBar healthBar;

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
            //ActiveSprite = spriteHolder.GetChild(0);

            activeSprite = null;
            foreach (Transform t in spriteHolder)
            {
                t.gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            Vector2 direction = (target.position - transform.position).normalized;

            Move(direction);
            RotateTowardsTarget(direction);
        }

        public void Initialize(EnemyType t)
        {
            SwitchType(t);
            health = maxHealth;
            healthBar ??= GetComponentInChildren<HealthBar>();
        }

        public void SetTarget(Entity target) => this.target = target.transform;


        public void SwitchType(EnemyType t) => Type = t;

        private void SetType(EnemyType t)
        {
            health = (health / maxHealth) * t.maxHealth;
            maxHealth = (int)t.maxHealth;
            speed = t.speed;
            strength = t.strength;
            defense = t.defense;

            if (null == t._sprite)
            {
                ActiveSprite = spriteHolder.GetChild(t.sprite);
                SetColour(t.colour);
            }
            else
            {
                ActiveSprite = spriteHolder.GetChild(2);
                ActiveSprite.GetComponent<SpriteRenderer>().sprite = t._sprite;
                ActiveSprite.transform.localScale = Vector3.one * t.spriteSize;
            }

            type = t;
        }

        private void SetColour(Color c)
        {
            SpriteRenderer[] sprites = new SpriteRenderer[] {
                activeSprite.GetChild(0).GetComponent<SpriteRenderer>(),
                activeSprite.GetChild(1).GetComponent<SpriteRenderer>()
            }; foreach (SpriteRenderer sr in sprites) sr.color = c;
        }

        private void RotateTowardsTarget()
        {
            _rb.MoveRotation(Quaternion.LookRotation(target.position));
        }

        private void RotateTowardsTarget(Vector2 direction)
        {
            if (direction == Vector2.zero) return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rb.MoveRotation(angle - 90f);
        }

        protected bool TakeDamage(float damage, EnemyType.Element element)
        {
            /* Apply type weaknesses to damage before deducting health */

            // Multiplier is 2 to the power of element count
            int advantageCount = type.advantageList.Count(e => e == element);
            int weaknessCount = type.weaknessList.Count(e => e == element);

            damage *= Mathf.Pow(2, advantageCount);
            damage /= Mathf.Pow(2, weaknessCount);

            if (TakeDamage(damage))
            {
                healthBar.UpdateHealthBar(health / maxHealth);
                return true;
            }
            return false;
        }

        protected void InteractWithElement(EnemyType.Element element) => Type = element switch {
            EnemyType.Element.Water => Type.wetList,
            EnemyType.Element.Fire => Type.wetList,
            _ => Type
        };

        public void HitWithProjectile(Projectile projectile)
        {
            if (type.critical && projectile.elements.Count > 1)
            {
                TakeDamage(projectile.dmg);
                /*
                 * int count = projectile.elements.Count;
                 * foreach (var element in projectile.elements){
                 *  TakeDamage(projectile.dmg / count, element);
                 * }
                 */
                return;
            }


            // Use first element if exists, otherwise Normal
            var element = projectile.elements.Count > 0
                ? projectile.elements[0]
                : EnemyType.Element.Normal;

            TakeDamage(projectile.dmg, element);
        }

        public override void Die() {
            Debug.Log("Death handled by ENEMY script");
            EnemySpawner._Instance.DespawnEnemy(this);
        }


        public void Kill()
        {
            Debug.Log($"[Enemy] Kill() called on {gameObject.name}");
            Die();
        }


    }



}