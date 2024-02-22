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
        bool healthMaxed = playerScript.currentWeapon.healthMaxValue >= playerScript.currentWeapon.healthUpgradeMax;

        // Deactivate all item cards
        foreach (var card in itemCards)
        {
            card.SetActive(false);
        }

        // If all upgrades are maxed, resume the game
        if (cooldownMaxed && speedMaxed && moveSpeedMaxed)
        {
            Time.timeScale = 1f;
            return;
        }

        // List to store the available cards
        List<string> availableTags = new List<string>();

        // Add tags for attributes that are not maxed out
        if (!cooldownMaxed)
        {
            availableTags.Add("Cooldown");
        }
        if (!speedMaxed)
        {
            availableTags.Add("Speed");
        }
        if (!moveSpeedMaxed)
        {
            availableTags.Add("MoveSpeed");
        }
        if (!healthMaxed)
        {
            availableTags.Add("Health");
        }

        // Activate a random card from the available tags
        if (availableTags.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTags.Count);
            ActivateCardWithTag(availableTags[randomIndex]);
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