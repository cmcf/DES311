using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class EnemyProjectile : MonoBehaviour
{
    Transform target;
    float damageAmount;
    [SerializeField] float moveSpeed = 5f;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDamage(float amount)
    {
        damageAmount = amount;
    }

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            float distanceThisFrame = Time.deltaTime * moveSpeed;
            if (distanceThisFrame >= Vector3.Distance(transform.position, target.position))
            {
                // If the projectile has reached the target, deal damage and destroy the projectile
                DealDamage();
                Destroy(gameObject);
            }
            else
            {
                transform.Translate(direction * distanceThisFrame, Space.World);
            }
        }
        else
        {
            // If the target is null, just move the projectile forward
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }

    void DealDamage()
    {
        // Check if the target has a damageable component
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Deal damage to the target
            damageable.Damage(damageAmount);
        }
    }
}
