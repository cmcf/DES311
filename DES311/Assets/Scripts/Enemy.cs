using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damage;

public class Enemy : MonoBehaviour, IDamageable
{
    public Transform player;
    public float moveSpeed = 5f; 
    public float lerpSpeed = 0.1f;

    public float maxHealth = 100f;
    public float currentHealth;

    bool hit = false;
    // Gets the Position property from IDamageable interface
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    public void Damage(float damage)
    {
        // Only deals damage if damage has not already been dealt
        if(!hit)
        {
            Debug.Log("Hit");
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }

            hit = true;
        }
    }

    // Reset flags when enemy is hit by a new bullet
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hit = false;
        }
    }

    void Die()
    {
        
        Destroy(gameObject);
    }
    void MoveTowardsPlayer()
    {
        // Calculate location for the enemy to move towards
        Vector3 moveLocation = player.position;

        // Smoothly move towards the player
        transform.position = Vector3.Lerp(transform.position, moveLocation, lerpSpeed * Time.deltaTime);

        // Enemy rotates towards player
        transform.LookAt(player);
    }
}


