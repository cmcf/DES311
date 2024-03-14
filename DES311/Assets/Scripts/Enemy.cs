using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Damage;

public class Enemy : MonoBehaviour, IDamageable
{
    private static List<Enemy> allEnemies = new List<Enemy>();

    Transform playerLocation;
    Player player;
    NavMeshAgent nav;
    EnemyManager enemyManager;
    Animator anim;
    [SerializeField] GameObject powerUpPrefab;

    public Color flashColour = Color.green;

    Renderer enemyRenderer;
    Color originalColor;

    // Chance of pickup spawning 
    [SerializeField] float spawnProbability = 0.2f;

    [Header("Movement")]
    float moveSpeed;
    [SerializeField] float minMoveSpeed = 1f;
    [SerializeField] float maxMoveSpeed = 5f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float stoppingDistance = 2.2f;
    [SerializeField] float levelUpStatIncrease = 0.5f;

    [Header("Health")]
    public float defaultHealth = 30f;
    [SerializeField] float increaseHealthAmount = 5f;
    public float currentHealth;

    [Header("Damage")]
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float damageAmount = 10f;
    [SerializeField] int XPAmount = 25;

    private Vector3 lastPlayerPosition;

    bool hit = false;
    bool reachedPlayer = false;
    bool hasIncreasedHealth = false;
    // Flag to indicate whether the enemy has been destroyed
    bool isDestroyed = false;

    // Gets the Position property from IDamageable interface
    public float Health { get; set; }

    void Awake()
    {
        allEnemies.Add(this);
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        originalColor = enemyRenderer.material.color;
        enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.RegisterEnemy(this);
        }
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        nav = GetComponent<NavMeshAgent>();
        nav.speed = moveSpeed;
        currentHealth = defaultHealth;

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
 
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
            // Change colour of enemy 
            StartCoroutine(HitEffect());
            hit = true;
        }
    }

     IEnumerator HitEffect()
     {
        // Change enemy colour to the set flash colour
        enemyRenderer.material.color = flashColour;

        // Wait until delay has ended
        yield return new WaitForSeconds(0.1f);

        // Revert enemy colour back to original
        enemyRenderer.material.color = Color.white;
     }

    // Reset flags when enemy is hit by a new bullet
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("FireProjectile"))
        {
            hit = false;
        }
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    void Die()
    {
        // Increase player XP
        GameManager.instance.IncreaseXP(XPAmount);
        // Checks if the pickup should spawn based on the probability value
        if (Random.value < spawnProbability && player.currentLevel >= 3)
        {
            // Spawns the pickup at the enemy's position
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
        isDestroyed = true;
        // Destroy the enemy gameObject
        Destroy(gameObject);
    }
    void MoveAndRotateTowardsPlayer()
    {
        // Calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, playerLocation.position);

        // Check if the enemy is near the player
        if (distanceToPlayer < stoppingDistance)
        {
            // Stop moving
            nav.isStopped = true;
            // Play idle animation
            anim.SetBool("HasStopped", true);

            // Rotate towards the player only if not reached player yet
            if (!reachedPlayer)
            {
                Vector3 direction = (playerLocation.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            // Set flag indicating that the enemy has reached the player
            reachedPlayer = true;

            // Attack the player
            AttackPlayer();

            // Store the player's current position
            lastPlayerPosition = playerLocation.position;
        }
        else
        {
            // If not reached player yet, set destination to the player's position
            if (!reachedPlayer)
            {
                nav.SetDestination(playerLocation.position);
                anim.SetBool("HasStopped", false);
            }
            else
            {
                // Check if the player has moved away
                if (Vector3.Distance(lastPlayerPosition, playerLocation.position) > stoppingDistance)
                {
                    // Stop the attack
                    StopAttack();

                    // Resume moving
                    nav.isStopped = false;
                    // Play attack animation
                    anim.SetBool("HasStopped", false);
                    // Reset reachedPlayer flag
                    reachedPlayer = false;
                }
            }
        }
    }

    void AttackPlayer()
    {
        if (player.isDead) { return; }
        // Play attack animation
        anim.SetBool("IsAttacking", true);
        // Reset attack animation
        StartCoroutine(ResetIsAttackingAfterDelay(attackCooldown));
    }

    void StopAttack()
    {
        // Stop the attack animation
        anim.SetBool("IsAttacking", false);
    }

    void DamagePlayer()
    {
        // Calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, playerLocation.position);

        // Check if the player is within the attack range
        if (distanceToPlayer <= stoppingDistance)
        {
            // Perform the attack by dealing damage to the player
            player.Damage(damageAmount);
        }
    }

    IEnumerator ResetIsAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Reset IsAttacking after the delay
        anim.SetBool("IsAttacking", false);
    }

    public void IncreaseEnemyStats()
    {
        Debug.Log("Enemy levelled up");

        // Increase minMoveSpeed
        minMoveSpeed += levelUpStatIncrease;

        // Cap minMoveSpeed at 5
        minMoveSpeed = Mathf.Min(minMoveSpeed, 5);

        // Set moveSpeed within the new range
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    public void IncreaseHealth(float amount)
    {
        if (!hasIncreasedHealth)
        {
            currentHealth += amount;
            hasIncreasedHealth = true;
        }
       
    }

}



