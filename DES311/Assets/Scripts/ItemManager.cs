using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] weaponCards;
    public PlayerMovement playerScript;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerMovement>();
    }
    public void DisplayItemChoice()
    {
        // Ensure there are items in the array
        if (weaponCards.Length == 0)
        {
            Debug.LogWarning("No item cards assigned.");
            return;
        }

        // Check if the current weapon has reached the maximum upgrade for each attribute
        bool cooldownMaxed = playerScript.currentWeapon.cooldown <= playerScript.currentWeapon.minCooldown;
        bool speedMaxed = playerScript.currentWeapon.speed >= playerScript.currentWeapon.maxSpeed;

        // Remove cards from the array if the corresponding attribute is maxed out
        List<GameObject> remainingCards = new List<GameObject>(weaponCards);

        if (cooldownMaxed)
        {
            remainingCards.RemoveAll(card => card.CompareTag("Cooldown"));
        }
        if (speedMaxed)
        {
            remainingCards.RemoveAll(card => card.CompareTag("Speed"));
        }

        // Choose a random index from the remaining cards
        if (remainingCards.Count > 0)
        {
            int randomIndex = Random.Range(0, remainingCards.Count);

            // Deactivate all item cards
            foreach (var card in weaponCards)
            {
                card.SetActive(false);
            }

            // Activate the chosen item card
            remainingCards[randomIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("No valid item cards left.");
        }
    }
}
