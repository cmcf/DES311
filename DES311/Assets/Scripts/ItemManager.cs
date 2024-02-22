using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemCards;
    PlayerMovement playerScript;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerMovement>();
    }
    public void DisplayItemChoice()
    {
        // Ensure there are items in the array
        if (itemCards.Length == 0)
        {
            Debug.LogWarning("No item cards assigned.");
            return;
        }

        // Check if the current weapon has reached the maximum upgrade for each attribute
        bool cooldownMaxed = playerScript.currentWeapon.cooldown <= playerScript.currentWeapon.minCooldown;
        bool speedMaxed = playerScript.currentWeapon.speed >= playerScript.currentWeapon.maxSpeed;

        // Deactivate all item cards
        foreach (var card in itemCards)
        {
            card.SetActive(false);
        }

        // If both cooldown and speed are maxed, resume the game
        if (speedMaxed && cooldownMaxed)
        {
            Time.timeScale = 1f;
            return;
        }

        // If not, activate the corresponding card
        if (!cooldownMaxed && !speedMaxed)
        {
            // Choose a random index
            int randomIndex = Random.Range(0, itemCards.Length);

            // Activate the chosen item card
            itemCards[randomIndex].SetActive(true);
        }
        else if (cooldownMaxed && !speedMaxed)
        {
            // Activate the speed card if cooldown is maxed
            foreach (var card in itemCards)
            {
                if (card.CompareTag("Speed"))
                {
                    card.SetActive(true);
                    break;
                }
            }
        }
        else if (!cooldownMaxed && speedMaxed)
        {
            // Activate the cooldown card if speed is maxed
            foreach (var card in itemCards)
            {
                if (card.CompareTag("Cooldown"))
                {
                    card.SetActive(true);
                    break;
                }
            }
        }
    }
}