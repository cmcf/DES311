using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemCards;
    PlayerMovement playerScript;

    private static System.Random rng = new System.Random();
    private Dictionary<GameObject, Vector3> initialCardPositions = new Dictionary<GameObject, Vector3>();

    [SerializeField] float cardOffset = 500f;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerMovement>();
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
        else
        {
            Debug.LogWarning("itemCards array is null.");
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

        // Shuffle the available tags to randomize the selection
        Shuffle(availableTags);

        // Activate two random cards from the available tags
        int numCardsToDisplay = Mathf.Min(2, availableTags.Count);
        for (int i = 0; i < numCardsToDisplay; i++)
        {
            ActivateCardWithTag(availableTags[i], i);
        }
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

    void ResetCardPositions()
    {
        // Resets the positions of all cards to their initial positions
        foreach (var kvp in initialCardPositions)
        {
            kvp.Key.transform.localPosition = kvp.Value;
        }
    }
}