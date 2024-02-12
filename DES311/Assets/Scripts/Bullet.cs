using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxDistance = 100f;
    private Vector3 initialPosition;
    float damageAmount = 15f;
    Enemy enemy;
    bool hasHitEnemy = false;
    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the traveled distance
        float traveledDistance = Vector3.Distance(transform.position, initialPosition);

        // Check if the bullet has traveled far enough
        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            other.GetComponent<Enemy>().Damage(damageAmount);

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}

