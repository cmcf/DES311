using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemCards;
    PlayerMovement playerScript;
    Player playerStats;

    private static System.Random rng = new System.Random();
    private Dictionary<GameObject, Vector3> initialCardPositions = new Dictionary<GameObject, Vector3>();

    [SerializeField] float cardOffset = 500f;

    void Start()
    {
        playerStats = FindObjectOfType<Player>();
    }

    void Shuffle(List<string> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void HideItemSelection()
    {
        // Check if itemCards array is not null
        if (itemCards != null)
        {
            // Iterate over all item cards and hide each of them if they are not null
            foreach (var card in itemCards)
            {
                if (card != null)
                {
                    card.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Null item card found in itemCards array.");
                }
            }
        }

        // Hide the entire item selection interface
        gameObject.SetActive(false);
    }
    public void DisplayItemChoice()
    {
        // Ensure there are items in the array
        if (itemCards.Length == 0)
        {
            Debug.LogWarning("No item cards assigned.");
            return;
        }

        ResetCardPositions();

        playerScript = FindObjectOfType<PlayerMovement>();

        // Check if the current weapon has reached the maximum upgrade for each attribute
        bool cooldownMaxed = playerScript.currentLoadout.cooldown <= playerScript.currentLoadout.minCooldown;
        bool speedMaxed = playerScript.currentLoadout.speed >= playerScript.currentLoadout.maxSpeed;
        bool moveSpeedMaxed = playerScript.currentLoadout.moveSpeed >= playerScript.currentLoadout.maxMoveSpeed;
        bool healthMaxed = playerScript.currentLoadout.healthMaxValue >= playerScript.currentLoadout.healthUpgradeMax;

        // Check if "FireCard" is already equipped and player's level is above 3
        bool fireCardEquipped = CheckIfFireCardEquipped();
        bool stoneCardEquipped = CheckIfStoneCardEquipped();
        bool playerAboveRequiredLevel = CheckIfPlayerIsAboveRequiredLevel();

        // Deactivate all item cards
        foreach (var card in itemCards)
        {
            card.SetActive(false);
        }

        // If all upgrades are maxed, resume the game
        if (cooldownMaxed && speedMaxed && moveSpeedMaxed && healthMaxed)
        {
            Time.timeScale = 1f;
            return;
        }

        // List to store the available tags
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

        // Add fire card if not already equipped and player is above the required level
        if (!fireCardEquipped && playerAboveRequiredLevel)
        {
            availableTags.Add("FireCard");
        }

        // Add fire card if not already equipped and player is above the required level
        if (!stoneCardEquipped && playerAboveRequiredLevel)
        {
            availableTags.Add("StoneCard");
        }

        // Shuffle the available tags to randomize the selection
        Shuffle(availableTags);

        // Activate two random cards from the available tags
        int numCardsToDisplay = Mathf.Min(2, availableTags.Count);
        for (int i = 0; i < numCardsToDisplay; i++)
        {
            ActivateCardWithTag(availableTags[i], i);
        }
    }

    bool CheckIfStoneCardEquipped()
    {
        if (playerScript.HasProjectileWithTag("StoneProjectile"))
        {
            return true;
        }
        return false;
    }

    void ActivateCardWithTag(string tag, int index)
    {
        // Activate the card with the specified tag
        foreach (var card in itemCards)
        {
            if (card.CompareTag(tag))
            {
                // Stores the initial position of the card if it's not already stored
                if (!initialCardPositions.ContainsKey(card))
                {
                    initialCardPositions.Add(card, card.transform.localPosition);
                }

                // Calculates the horizontal position for the card based on the index
                float xOffset = index * cardOffset;
                card.transform.localPosition = new Vector3(initialCardPositions[card].x + xOffset, initialCardPositions[card].y, initialCardPositions[card].z);

                card.SetActive(true);
                return;
            }
        }
    }

    bool CheckIfFireCardEquipped()
    {
        if (playerScript.HasProjectileWithTag("FireProjectile"))
        {
            return true;
        }
        return false;
    }

    bool CheckIfPlayerIsAboveRequiredLevel()
    {
        if (playerStats.GetCurrentLevel() >= 2)
        {
           return true;
        }
        return false;
    }

    void ResetCardPositions()
    {
        // Resets the positions of all cards to their initial positions
        foreach (var kvp in initialCardPositions)
        {
            kvp.Key.transform.localPosition = kvp.Value;
        }
    }
}