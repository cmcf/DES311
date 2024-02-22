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
        bool moveSpeedMaxed = playerScript.currentWeapon.moveSpeed >= playerScript.currentWeapon.maxMoveSpeed;

        // Deactivate all item cards
        foreach (var card in itemCards)
        {
            card.SetActive(false);
        }

        // If both cooldown and speed are maxed, resume the game
        if (speedMaxed && cooldownMaxed && moveSpeedMaxed)
        {
            Time.timeScale = 1f;
            return;
        }

        // Activate the corresponding card based on the attribute that is not maxed
        if (!moveSpeedMaxed)
        {
            // Activate the cooldown card
            ActivateCardWithTag("MoveSpeed");
        }
        else if (!speedMaxed)
        {
            // Activate the speed card
            ActivateCardWithTag("Speed");
        }
        else if (!cooldownMaxed)
        {
            // Activate the move speed card
            ActivateCardWithTag("Cooldown");
        }
    }

    void ActivateCardWithTag(string tag)
    {
        // Activate the card with the specified tag
        foreach (var card in itemCards)
        {
            if (card.CompareTag(tag))
            {
                card.SetActive(true);
                return;
            }
        }
    }
}