using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Damage;

public class Enemy : MonoBehaviour, IDamageable
{
    public Player player;
    public Color flashColour;
    public Color originalColour = Color.white;
    public Renderer enemyRenderer;
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject deathVFX;
    
    public GameObject powerUpPrefab;
    // Gets the Position property from IDamageable interface
    public float Health { get; set; }

    public float maxHealth = 35f;
    public float currentHealth;

    public float slowAmount;
    public float moveSpeed;
    public int XPAmount = 25;
    // Chance of pickup spawning 
    public float spawnProbability = 0.01f;

    [SerializeField] bool isBoss = false;

    bool isDestroyed = false;
    bool hasIncreasedHealth = false;
    bool hit = false;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        player = FindObjectOfType<Player>();
          
    }
    public void Damage(float damage)
    {
        // Only deals damage if damage has not already been dealt
        if (!hit)
        {
            currentHealth -= damage;
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
            // Change colour of enemy 
            StartCoroutine(HitEffect());
            hit = true;
        }
    }
    // Reset flags when enemy is hit by a new bullet
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("FireProjectile") || collision.gameObject.CompareTag("StoneProjectile") || collision.gameObject.CompareTag("Water"))
        {
            hit = false;
        }
    }

    public virtual void SlowdownEffect(float amount, float duration)
    {
        // Adjust the movement speed
        moveSpeed -= amount;
    }

    IEnumerator HitEffect()
    {
        // Play the sound when enemy is hit
        FindObjectOfType<AudioManager>().Play("EnemyHit");
        // Change enemy colour to the set flash colour
        enemyRenderer.material.color = flashColour;

        // Wait until delay has ended
        yield return new WaitForSeconds(0.1f);

        // Revert enemy colour back to original
        enemyRenderer.material.color = originalColour;
    }

    IEnumerator LoadWinScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the enemy object
        Destroy(gameObject);
        GameManager.instance.LevelComplete();
        
        Time.timeScale = 0f;
    }

    public virtual void Die()
    {
        if (isBoss)
        {
            FindObjectOfType<AudioManager>().Play("EnemyDeath");
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            player.DisablePlayerMovement();
            
            StartCoroutine(LoadWinScene(1f));
        }
        else
        {
            // Increase player XP
            GameManager.instance.IncreaseXP(XPAmount);
            // Spawn powerup by chance and only if player is above level 3
            if (player != null && Random.value < spawnProbability && player.currentLevel >= 3)
            {
                // Spawns the pickup at the enemy's position
                Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            }
            FindObjectOfType<AudioManager>().Play("EnemyDeath");
            isDestroyed = true;
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            // Destroy the enemy gameObject
            DestroyEnemy();
        }
    }

    public void IncreaseHealth(float amount)
    {
        if (!hasIncreasedHealth)
        {
            currentHealth += amount;
            hasIncreasedHealth = true;
        }

    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
