using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float damageAmount = 15;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float projectileHeight;

    public void Launch(Vector3 Destination)
    {
        float gravity = Physics.gravity.magnitude;
        float halfFlightTime = Mathf.Sqrt(projectileHeight * 2) / gravity;

        // Calculate the direction towards the destination
        Vector3 projectileDestination = Destination - transform.position;
        projectileDestination.y = 0; // Disregard the vertical component

        // Calculate the horizontal distances
        float horizontalDistance = projectileDestination.magnitude;

        // Calculate the forward direction
        Vector3 forwardDirection = projectileDestination.normalized;

        // Calculate the up speed
        float upSpeed = halfFlightTime * gravity;

        // Calculate the forward speed
        float forwardSpeed = horizontalDistance / (2 * halfFlightTime);

        // Scale down the forward speed to control the distance
        float projectileDistance = 5;
        float distanceScaleFactor = projectileDistance / horizontalDistance;
        forwardSpeed *= distanceScaleFactor;

        // Calculate the total flight velocity
        Vector3 flightVelocity = forwardDirection * forwardSpeed + Vector3.up * upSpeed;

        // Apply the flight velocity to the rigidbody
        rb.velocity = flightVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage(other.transform);
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);    
        }
        
    }

    void DealDamage(Transform target)
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
