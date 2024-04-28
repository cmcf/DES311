using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    // Reference to the particle effect for the wipeout effect
    public GameObject pickupEffect;
    // Array of card prefabs to display
    public GameObject[] powerupCardPrefabs;
    // Reference to the EnemyManager
    public EnemyManager enemyManager;  
    // Reference to the card that is activated
    private GameObject activatedCard;

    // Reference to the Enemy script
    public MeleeEnemy enemyScript;

    public Player playerStats;

    float amount = 20f;

    public PlayerMovement playerLoadout;

    [SerializeField] int XPIncreaseAmount = 500;
    [SerializeField] int playerHealthIncreaseAmount = 10;
    [SerializeField] float decreaseSpeedAmount = 0.5f;

    // Singleton instance
    private static PickupManager instance;

    // Public method to access the singleton instance
    public static PickupManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Find the instance in the scene if it hasn't been set
                instance = FindObjectOfType<PickupManager>();

                // If still not found, log an error
                if (instance == null)
                {
                    Debug.LogError("No instance of PickupManager found in the scene.");
                }
            }
            return instance;
        }
    }

    // Method to select and activate a power-up card at random
    public void ActivatePowerUp()
    {
        // Check if there are any card prefabs available
        if (powerupCardPrefabs.Length == 0)
        {
            Debug.LogWarning("No card prefabs assigned!");
            return;
        }
        // Game is pause while card is active
        Time.timeScale = 0f;

        // Generate a random index to select a card prefab
        int randomIndex = Random.Range(0, powerupCardPrefabs.Length);

        // Enable the randomly selected card prefab and make it visible
        for (int i = 0; i < powerupCardPrefabs.Length; i++)
        {
            bool isActive = i == randomIndex;
            powerupCardPrefabs[i].SetActive(isActive);

            // If the card is activated, store its reference
            if (isActive)
            {
                activatedCard = powerupCardPrefabs[i];
            }
        }

        // Instantiate the pick-up effect at the location of the power-up
        Instantiate(pickupEffect, transform.position, Quaternion.identity);
    }

    public void WipeoutEnemies()
    {
        Time.timeScale = 1f;
        // Removes all current enemies spawned in the level
        enemyManager.WipeOutEnemies();
        DeactivateCard();
    }

    public void IncreaseXP()
    {
        Time.timeScale = 1f;
        // Increases the players current XP
        playerStats.currentXP += XPIncreaseAmount;
        GameManager.Instance.IncreaseXP(XPIncreaseAmount);
        // Increases current enemies health
        enemyManager.IncreaseEnemyHealth(amount);
        DeactivateCard();
    }

    public void IncreaseHealth()
    {
        Time.timeScale = 1f;
        // Increases the players current health
        playerLoadout.currentLoadout.health += playerHealthIncreaseAmount;
        // Decreases movement speed
        playerLoadout.currentLoadout.moveSpeed -= decreaseSpeedAmount;
        DeactivateCard();  
    }

     void DeactivateCard()
     {
        // Disables the activated card
        if (activatedCard != null)
        {
            activatedCard.SetActive(false);
        }
        // Removes pickup
        gameObject.SetActive(false);
     }

    public void DeclineCard()
    {
        Time.timeScale = 1f;
        DeactivateCard();
    }
}
