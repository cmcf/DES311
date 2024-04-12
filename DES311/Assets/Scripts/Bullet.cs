using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class Bullet : MonoBehaviour
{
    private Vector3 initialPosition;
    [SerializeField] float damageAmount = 15f;

    [Header("Bullet Type")]
    [SerializeField] bool isLaser = false;
    [SerializeField] bool isStoneBullet = false;
    [SerializeField] bool isWaterBall = false;

    Enemy enemy;
    [SerializeField] float destroyDelay = 3f;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float destroyDelayAfterCollision = 0.2f;
    [SerializeField] float pushbackAmount = 2f;
    [SerializeField] float pushbackDuration = 0.5f;
    public string soundName;

    [SerializeField] float slowAmount = 0.5f;
    [SerializeField] float slowDuration = 10f;
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

                // If it's a stone bullet, apply pushback effect
                if (isStoneBullet)
                {
                    StartCoroutine(PushbackEnemy(other.transform));
                }
            }

            if (hitEffect != null)
            {
                // Instantiate the hit effect at the position of the collision
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            if (!isLaser)
            {
                // Disable the bullet's collider to prevent it from hitting multiple enemies
                GetComponent<Collider>().enabled = false;
            }
            if (isWaterBall)
            {
                enemy = other.GetComponent<Enemy>();

                ApplySlowEffect(enemy);

            }
            else
            {
                // Destroy the bullet immediately if it hits something other than an enemy
                Invoke(nameof(DestroyBullet), destroyDelayAfterCollision);
            }
        }

        IEnumerator PushbackEnemy(Transform enemyTransform)
        {
            // Store the original position of the enemy
            Vector3 originalPosition = enemyTransform.position;
            // Calculate the target position for pushback
            Vector3 targetPosition = enemyTransform.position - enemyTransform.forward * pushbackAmount;
            // Initialize time elapsed
            float elapsedTime = 0f;
            // Smoothly move the enemy back over the specified duration
            while (elapsedTime < pushbackDuration)
            {
                // Calculate interpolation factor
                float t = elapsedTime / pushbackDuration;
                if (enemyTransform != null)
                {
                    // Interpolate position
                    enemyTransform.position = Vector3.Lerp(originalPosition, targetPosition, t);
                    // Increment time elapsed
                    elapsedTime += Time.deltaTime;
                }

                yield return null;
            }

        }
      
    }
    void ApplySlowEffect(Enemy enemy)
    {
        // Apply slow effect
        enemy.SlowdownEffect(slowAmount, slowDuration);

    }


    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}

