using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Damage;

public class RangedEnemy : Enemy
{
    Transform playerLocation;
    NavMeshAgent nav;
    Animator anim;

    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] EnemyProjectile projectilePrefab;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stoppingDistance = 10f;
    [SerializeField] float rotationSpeed = 2f;


    [Header("Damage")]
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float damageAmount = 10f;
    private Vector3 lastPlayerPosition;

    public  bool reachedPlayer = false;
    bool canAttack = true;

   
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = moveSpeed;
        anim = GetComponent<Animator>();
        // Find the player GameObject and get its transform component
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        
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

            // Attack the player when range
            AttackPlayer();
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

    void AttackPlayer()
    {
        if (canAttack && projectilePrefab != null)
        {
            anim.SetBool("IsAttacking", true);
            // Shoot projectile
            StartCoroutine(ShootProjectileAfterDelay(1f));
        }
    }

    IEnumerator ShootProjectileAfterDelay(float delay)
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
   
        EnemyProjectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.Launch(playerLocation.position);

        anim.SetBool("IsAttacking", false);

        // Wait for the attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        // Reset the flag to allow the enemy to attack again
        canAttack = true;
    }
}
