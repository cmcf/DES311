using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Damage;

public class Enemy : MonoBehaviour, IDamageable
{
    public Transform playerLocation;
    Player player;
    NavMeshAgent agent;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 2f;

    [SerializeField] float stoppingDistance = 2.2f;

    [Header("Health")]
    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    [Header("Damage")]
    [SerializeField] float attackCooldown = 1f;
    float lastAttackTime = -Mathf.Infinity;
    [SerializeField] float damageAmount = 10f;

    bool hit = false;
    bool reachedPlayer = false;

    // Gets the Position property from IDamageable interface
    public float Health { get; set; }


    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
         // Find the player GameObject and get its transform component
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (playerLocation != null)
        {
            MoveAndRotateTowardsPlayer();
        }  
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
    void MoveAndRotateTowardsPlayer()
    {
        // Calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, playerLocation.position);

        // Check if the enemy is near the player
        if (distanceToPlayer < stoppingDistance)
        {
            // Stop moving and attack player
            agent.isStopped = true;
            AttackPlayer();

            // Rotate towards the player only if not reached player yet
            if (!reachedPlayer)
            {
                Vector3 direction = (playerLocation.position - transform.position).normalized;
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
                agent.SetDestination(playerLocation.position);
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

    void AttackPlayer()
    {
        // Enemy only attacks if the player is alive
        if (player.isDead)
        {
            return;
        }
        // Check if enough time has passed since the last attack
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Perform the attack
            player.Damage(damageAmount);

            // Update the last attack time to the current time
            lastAttackTime = Time.time;
        }
    }
}



