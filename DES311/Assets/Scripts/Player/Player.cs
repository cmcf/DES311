using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static Damage;

public class Player : MonoBehaviour, IDamageable
{
    public ItemManager itemManager;
    public PlayerMovement playerStats;
    public HUD playerHUD;
    EnemyManager enemyManager;

    [Header("Stats")]
    public int currentXP;
    public int requiredXP;
    [SerializeField] int requiredXPIncreaseRate = 150;
    public int currentLevel = 1;

    public bool isDead = false;
    public float Health { get; set; }

    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        playerStats = GetComponent<PlayerMovement>();
        // Enable the XPEvent
        GameManager.instance.XPEvent += HandleXP;
    }

    void OnDisable()
    {
        // Disable the from the XPEvent when the script is disabled
        GameManager.instance.XPEvent -= HandleXP;
    }
 
    // Function called when the XPEvent is invoked
    void HandleXP(int newXP)
    {
        // Increase current XP
        currentXP += newXP;
        // If players current XP is equal or more than the required XP, the player levels up
        if (currentXP >= requiredXP)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        // Pauses the game
        Time.timeScale = 0f;
        itemManager.DisplayItemChoice();
        // Players current level increases
        currentLevel++;
        if (enemyManager != null)
        {
            enemyManager.LevelUpEnemies();
        }
        // XP is reset
        currentXP = 0;
        // The amount of XP required to reach the next level is increased each level by the increase rate
        requiredXP += requiredXPIncreaseRate;
    }

    public void Damage(float damage)
    {
        Debug.Log("PlayerHit");
        
        playerStats.currentWeapon.health -= damage;
        playerHUD.UpdateHealthBar();
        if (playerStats.currentWeapon.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Dead");
        isDead = true;
    }
   
}
