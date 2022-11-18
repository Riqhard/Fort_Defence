using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;

    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    public virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        
    }

    public virtual Vector3 GenerateNextIdlePos(Vector3 initialIdleCordinates, float maxRange)
    {
        float randomXCoordinate = Random.Range(-maxRange, maxRange);
        float randomzCoordinate = Random.Range(-maxRange, maxRange);

        Vector3 coordinates = new Vector3(initialIdleCordinates.x + randomXCoordinate, 1, initialIdleCordinates.z + randomzCoordinate);
        return coordinates;
    }
}
