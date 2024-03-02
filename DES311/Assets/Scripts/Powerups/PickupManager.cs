using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    // Reference to the particle effect for the wipeout effect
    public GameObject wipeoutEffect;
    // Array of card prefabs to display
    public GameObject[] powerupCardPrefabs;
    // Reference to the EnemyManager
    public EnemyManager enemyManager;  
    // Reference to the card that is activated
    private GameObject activatedCard;

    public Player playerStats;

    public PlayerMovement playerLoadout;

    [SerializeField] int XPIncreaseAmount = 500;
    [SerializeField] int healthIncreaseAmount = 10;

    // Method to activate the power-up
    public void ActivatePowerUp()
    {
        // Check if there are any card prefabs available
        if (powerupCardPrefabs.Length == 0)
        {
            Debug.LogWarning("No card prefabs assigned!");
            return;
        }
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

        // Instantiate the wipeout effect at the location of the power-up
        Instantiate(wipeoutEffect, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter(Collider other)
    {
        // Triggers the powerup when picked up. 
        ActivatePowerUp();
    }

    public void WipeoutEnemies()
    {
        Time.timeScale = 1f;
        // Wipe out all current enemies in the scene
        enemyManager.WipeOutEnemies();
        // Disable the activated card GameObject associated with this power-up
        if (activatedCard != null)
        {
            activatedCard.SetActive(false);
        }
        // Removes pickup
        gameObject.SetActive(false);

    }

    public void IncreaseXP()
    {
        Time.timeScale = 1f;
        playerStats.currentXP += XPIncreaseAmount;
        GameManager.instance.IncreaseXP(XPIncreaseAmount);
        // Disable the activated card GameObject associated with this power-up
        if (activatedCard != null)
        {
            activatedCard.SetActive(false);
        }
        // Removes pickup
        gameObject.SetActive(false);
    }

    public void IncreaseHealth()
    {
        Time.timeScale = 1f;
        playerLoadout.currentLoadout.health += healthIncreaseAmount;
        // Disable the activated card GameObject associated with this power-up
        if (activatedCard != null)
        {
            activatedCard.SetActive(false);
        }
        // Removes pickup
        gameObject.SetActive(false);
    }
}
