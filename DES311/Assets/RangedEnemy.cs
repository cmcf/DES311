using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Damage;

public class RangedEnemy : MonoBehaviour, IDamageable
{
    Transform playerLocation;
    Player player;
    NavMeshAgent nav;
    Animator anim;

    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] GameObject projectilePrefab;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stoppingDistance = 10f;
    [SerializeField] float rotationSpeed = 2f;

    [Header("Health")]
    [SerializeField] float maxHealth = 30f;
    float currentHealth;

    [Header("Damage")]
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float damageAmount = 10f;
    private Vector3 lastPlayerPosition;

    bool isDestroyed = false;
    public  bool reachedPlayer = false;
    bool canAttack = true;

    // Gets the Position property from IDamageable interface
    public float Health { get; set; }

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = moveSpeed;

        // Find the player GameObject and get its transform component
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        player = FindObjectOfType<Player>();
        //StartCoroutine(AttackPlayerRepeatedly());
    }

    void Update()
    {
        if (playerLocation != null)
        {
            MoveAndRotateTowardsPlayer();
        }
    }

    void MoveAndRotateTowardsPlayer()
    {
        // Calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, playerLocation.position);

        // Rotate towards the player's position
        Rotate();

        // Check if the enemy is near the player
        if (distanceToPlayer < stoppingDistance)
        {
            // Stop moving
            nav.isStopped = true;

            // Set flag indicating that the enemy has reached the player
            reachedPlayer = true;
        }
        else
        {
            // If not reached player yet, set destination to the player's position
            if (!reachedPlayer)
            {
                nav.SetDestination(playerLocation.position);
            }
            else
            {
                // Check if the player has moved away
                if (Vector3.Distance(lastPlayerPosition, playerLocation.position) > stoppingDistance)
                {
                    // Resume moving
                    nav.isStopped = false;

                    // Reset reachedPlayer flag
                    reachedPlayer = false;
                }
            }
        }
    }

    void Rotate()
    {
        // Rotate towards the player's position
        Vector3 direction = (playerLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    IEnumerator AttackPlayerRepeatedly()
        {
            while (true)
            {
                if (canAttack && playerLocation != null && Vector3.Distance(transform.position, playerLocation.position) <= stoppingDistance)
                {
                    AttackPlayer();
                }
                yield return new WaitForSeconds(attackCooldown);
            }
        }

    void AttackPlayer()
    {
        canAttack = false; // Set the flag to prevent further attacks until cooldown is over
        StartCoroutine(ShootProjectileAfterDelay(0.5f)); // Adjust the delay as needed
    }

    IEnumerator ShootProjectileAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        }
        // Wait for the attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        // Reset the flag to allow the enemy to attack again
        canAttack = true;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDestroyed)
        {
            Die();
        }
    }

    void Die()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}
