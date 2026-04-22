using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    [Header("Stats")]
    protected float health;
    public float Health => health;

    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected float speed = 1.0f, strength = 1.0f, defense = 1.0f;

    protected Rigidbody2D _rb;
    public virtual void TakeDamage(float damage) => health -= damage;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Move()
    {
        _rb.linearVelocity = Vector2.up * speed * Time.deltaTime;
    }
}
