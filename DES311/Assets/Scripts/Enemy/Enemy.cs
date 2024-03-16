using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Damage;

public class Enemy : MonoBehaviour, IDamageable
{
    public Player player;
    public Color flashColour;
    public Renderer enemyRenderer;

    public GameObject powerUpPrefab;
    // Gets the Position property from IDamageable interface
    public float Health { get; set; }

    public float defaultHealth = 30f;
    public float currentHealth = 35f;

    public int XPAmount = 25;
    // Chance of pickup spawning 
    public float spawnProbability = 0.01f;

    bool isDestroyed = false;
    bool hasIncreasedHealth = false;
    bool hit = false;

    void Awake()
    {
        player = FindObjectOfType<Player>();
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
    // Reset flags when enemy is hit by a new bullet
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("FireProjectile"))
        {
            hit = false;
        }
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
        enemyRenderer.material.color = Color.white;
    }

    public virtual void Die()
    {
        // Increase player XP
        GameManager.instance.IncreaseXP(XPAmount);
        // Spawn powerup by chance and only if player is above level 3
        if (player != null && Random.value < spawnProbability && player.currentLevel >= 3)
        {
            // Spawns the pickup at the enemy's position
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
        isDestroyed = true;
        // Destroy the enemy gameObject
        Destroy(gameObject);
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
