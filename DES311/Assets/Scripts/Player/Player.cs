using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static Damage;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    public ItemManager itemManager;
    public PlayerMovement playerLoadout;
    EnemyManager enemyManager;
    public HUD playerHUD;
    Animator animator;
    CharacterController controller;

    [SerializeField] float endSceneLoadDelay = 1f;
    [Header("Text Components")]
    public TextMeshProUGUI currentLevelText;
  

    [Header("Stats")]
    public int currentXP;
    public int requiredXP;
    [SerializeField] int requiredXPIncreaseRate = 150;
    public int currentLevel = 1;

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
        // Enable vibration
        Handheld.Vibrate();
        // Health bar is updated with the current health amount
        playerHUD.UpdateHealthBar();

        if (playerLoadout.currentLoadout.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) { return; }
        isDead = true;
        // Disable the character controller
        controller.enabled = false;

        //Play death SFX
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        animator.SetTrigger("Dead");
       
        // load end scene after a delay
        Invoke(nameof(LoadEndLevel), endSceneLoadDelay);
    }

    void LoadEndLevel()
    {
        GameManager.instance.ResetGame();
        SceneManager.LoadScene(2);
    }
   
}
