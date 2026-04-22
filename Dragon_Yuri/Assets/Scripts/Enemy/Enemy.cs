using Assets.Scripts.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public void SetTarget(Entity target) => this.target = target.transform;

        public void SwitchType(EnemyType t) => Type = t;

        private void SetType(EnemyType t)
        {
            health = (health / maxHealth) * t.maxHealth;
            maxHealth = (int) t.maxHealth;
            speed = t.speed;
            strength = t.strength;
            defense = t.defense;
            type = t;
        }

        protected override void Start()
        {
            base.Start();
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
    }
}