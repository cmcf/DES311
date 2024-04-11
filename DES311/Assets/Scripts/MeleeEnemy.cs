using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Damage;

public class MeleeEnemy :  Enemy
{
    private static List<MeleeEnemy> allEnemies = new List<MeleeEnemy>();

    Transform playerLocation;
    
    NavMeshAgent nav;
    EnemyManager enemyManager;
    Animator anim;

    [Header("Movement")]
    float originalMoveSpeed;
    [SerializeField] float minMoveSpeed = 1f;
    [SerializeField] float maxMoveSpeed = 5f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float stoppingDistance = 2.2f;
    [SerializeField] float levelUpStatIncrease = 0.5f;

    [Header("Damage")]
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float damageAmount = 10f;

    private Vector3 lastPlayerPosition;
    bool reachedPlayer = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.RegisterEnemy(this);
        }
        base.moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        nav = GetComponent<NavMeshAgent>();
        nav.speed = moveSpeed;
        originalMoveSpeed = moveSpeed;
        currentHealth = maxHealth;

        // Find the player GameObject and get its transform component
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;

        allEnemies.Add(this);
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
                if (Vector3.Distance(lastPlayerPosition, playerLocation.position) < stoppingDistance)
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

    public override void SlowdownEffect(float amount, float duration)
    {
        base.SlowdownEffect(amount, duration);

        // Calculates the new movement speed after applying the slow effect
        float newMoveSpeed = moveSpeed - amount;

        // Clamps the movement speed to ensure it doesn't go below the min speed
        float minMoveSpeed = 0.8f;
        moveSpeed = Mathf.Max(newMoveSpeed, minMoveSpeed);

        // Update NavMeshAgent's speed to match the adjusted move speed
        nav.speed = moveSpeed;

        SlowDownAnimation();

        // Start coroutine to restore speed
        StartCoroutine(RestoreMoveSpeed(duration));
    }
    void SlowDownAnimation()
    {
        // Slows down speed of the movement animation
        anim.SetFloat("Speed", 0.8f); 
    }

    IEnumerator RestoreMoveSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        // Restore original movement speed
        nav.speed = originalMoveSpeed;
    }

    void AttackPlayer()
    {
        if (base.player.isDead) { return; }

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
        // Play bite attack sound
        FindObjectOfType<AudioManager>().Play("BiteAttack");

        // Calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, playerLocation.position);

        // Check if the player is within the attack range
        if (distanceToPlayer <= stoppingDistance)
        {
            // Perform the attack by dealing damage to the player
            base.player.Damage(damageAmount);
            
        }
    }

    IEnumerator ResetIsAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Reset IsAttacking after the delay
        anim.SetBool("IsAttacking", false);
    }

}



