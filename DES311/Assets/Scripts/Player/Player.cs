using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static Damage;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : MonoBehaviour, IDamageable
{
    public ItemManager itemManager;
    public PlayerMovement playerLoadout;
    EnemyManager enemyManager;
    public HUD playerHUD;
    Animator animator;
    CharacterController controller;
    [SerializeField] Canvas deathCanvas;
    [SerializeField] Button settingsButton;
    Points pointsScript;

    [SerializeField] float endSceneLoadDelay = 1f;
    [Header("Text Components")]
    public TextMeshProUGUI currentLevelText;
  

    [Header("Stats")]
    public int currentXP;
    public int requiredXP;
    [SerializeField] int requiredXPIncreaseRate = 150;

    public int currentLevel = 1;
    public int enemyDeathCounter = 1;

    public int points = 10;

    public bool isDead = false;
    public float Health { get; set; }

    void Start()
    {
        controller= GetComponent<CharacterController>();
        animator = GetComponent<Animator>();    
        enemyManager = FindObjectOfType<EnemyManager>();
        playerLoadout = GetComponent<PlayerMovement>();
        // Enable the XPEvent
        GameManager.instance.XPEvent += HandleXP;
        currentLevelText.text = "Level: " + currentLevel.ToString();
        Time.timeScale = 1f;
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
        GameManager.instance.AddEnemy(enemyDeathCounter);
        // If players current XP is equal or more than the required XP, the player levels up
        if (currentXP >= requiredXP)
        {
            LevelUp();
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    void LevelUp()
    {
        // Pauses the game
        Time.timeScale = 0f;
        itemManager.DisplayItemChoice();
        // Players current level increases
        currentLevel++;
        GameManager.instance.AddCoins(points);
        if (enemyManager != null)
        {
            enemyManager.LevelUpEnemies();
        }
        // XP is reset
        currentXP = 0;
        // Updates current level text
        currentLevelText.text = "Level: " + currentLevel.ToString();

        // The amount of XP required to reach the next level is increased each level by the increase rate
        requiredXP += requiredXPIncreaseRate;
    }

    public void Damage(float damage)
    { 
        if (isDead) { return ; }
        FindObjectOfType<AudioManager>().Play("PlayerHit");
        // Current health is decreased by the damage received
        playerLoadout.currentLoadout.health -= damage;

        if (Settings.instance != null && Settings.instance.vibrationOn)
        {
            // Enable vibration
            Handheld.Vibrate();
        }
        // Health bar is updated with the current health amount
        playerHUD.UpdateHealthBar();

        if (playerLoadout.currentLoadout.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        pointsScript = FindObjectOfType<Points>();
        Spawner spawnerScript = FindObjectOfType<Spawner>();
        // Stops enemies spawning when player dies
        if (spawnerScript != null)
        {
            spawnerScript.canSpawn = false;
        }
        if (pointsScript != null)
        {
            pointsScript.UpdatePointsText();
        }
        
        if (isDead) { return; }
        isDead = true;
        DisablePlayerMovement();

        //Play death SFX
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        animator.SetTrigger("Dead");

        if (settingsButton != null)
        {
            settingsButton.enabled = false;
        }
        // load end scene after a delay
        Invoke(nameof(LoadEndLevel), endSceneLoadDelay);
    }

    public void DisablePlayerMovement()
    {
        // Disable the character controller
        controller.enabled = false;
    }

    void LoadEndLevel()
    {
        Time.timeScale = 0f;
        deathCanvas.enabled = true;
    }
   
}
