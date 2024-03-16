using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class Bullet : MonoBehaviour
{
    private Vector3 initialPosition;
    [SerializeField] float damageAmount = 15f;
    MeleeEnemy enemy;
    [SerializeField] float destroyDelay = 3f;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float destroyDelayAfterCollision = 0.2f;
    public string soundName;
    void Start()
    {
        // Play the sound when the bullet is fired
        FindObjectOfType<AudioManager>().Play(soundName);

        initialPosition = transform.position;
        // Call the DestroyActor method after a delay
        Invoke(nameof(DestroyBullet), destroyDelay);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Check if the target has a damageable component
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Deal damage to the target
                damageable.Damage(damageAmount);
            }

            if (hitEffect!= null)
            {
                // Instantiate the hit effect at the position of the collision
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Invoke(nameof(DestroyBullet), destroyDelay);
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

