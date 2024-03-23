using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class Splat : MonoBehaviour
{
    [SerializeField] float damageAmount = 5f;

    bool canDealDamage = true; 
    float damageCooldown = 0.5f;
    float lastDamageTime = -Mathf.Infinity;

    void OnTriggerStay(Collider other)
    {
        // Only deals damage to player if cooldown has passed
        if (other.CompareTag("Player") && Time.time >= lastDamageTime + damageCooldown)
        {
            // Deals damage to player if hit
            DealDamage(other.transform);

            // Update the last damage time
            lastDamageTime = Time.time;
        }
    }

    void DealDamage(Transform target)
    {
        // Check if the player has a damageable component
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Deal damage to the player
            damageable.Damage(damageAmount);
        }
    }
}
