using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 initialPosition;
    [SerializeField] float damageAmount = 15f;
    Enemy enemy;
    [SerializeField] float destroyDelay = 3f;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float destroyDelayAfterCollision = 0.2f;
    void Start()
    {
        initialPosition = transform.position;
        // Call the DestroyActor method after a delay
        Invoke(nameof(DestroyBullet), destroyDelay);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            other.GetComponent<Enemy>().Damage(damageAmount);

            if (hitEffect!= null)
            {
                // Instantiate the hit effect at the position of the collision
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        else
        {
            Invoke(nameof(DestroyBullet), destroyDelayAfterCollision);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

}

