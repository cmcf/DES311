using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Damage;

public class Enemy : MonoBehaviour, IDamageable
{
    public Transform player;
    NavMeshAgent agent;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    float stoppingDistance = 2f;

    public float maxHealth = 100f;
    public float currentHealth;

    bool hit = false;
    bool reachedPlayer = false;
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
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    public void Damage(float damage)
    {
        // Only deals damage if damage has not already been dealt
        if (!hit)
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
        // Calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the enemy is near the player
        if (distanceToPlayer < stoppingDistance)
        {
            // Stop moving
            agent.isStopped = true;

            // Rotate towards the player only if not reached player yet
            if (!reachedPlayer)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            // Set flag indicating that the enemy has reached the player
            reachedPlayer = true;
        }
        else
        {
            // If not reached player yet, set destination to the player's position
            if (!reachedPlayer)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                // Resume moving
                agent.isStopped = false;

                // Reset reachedPlayer flag
                reachedPlayer = false;
            }
        }
    }
}



