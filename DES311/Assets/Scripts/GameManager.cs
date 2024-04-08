using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public WeaponItem playerLoadout;
    PlayerMovement playerStats;
    ShopItem item;
    // Handles XP events
    public delegate void XPHandler(int amount);
    // Event is triggered when called from another script
    public event XPHandler XPEvent;

    public int currentEnemiesKilled;
    public int totalEnemiesKilled;

    public int currentCredits;
    public int totalCredits;

    string totalPointsKey = "TotalPoints";
    string totalCreditsKey = "TotalCoins";

    public GameObject itemDisplayObject; // Reference to the game object with the ItemDisplay script

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only be one GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Load total points from PlayerPrefs
        totalEnemiesKilled = PlayerPrefs.GetInt(totalPointsKey, 0);
        totalCredits = PlayerPrefs.GetInt(totalCreditsKey, 0);
    }

    public void IncreaseXP(int amount)
    {
        // If not null, increase XP by amoung gained
        XPEvent?.Invoke(amount);
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public int GetTotalCredits()
    {
        return totalCredits;
    }

    public void AddCoins(int points)
    {
        currentCredits += points;
        totalCredits += points;
        // Save total coins  earned
        PlayerPrefs.SetInt(totalCreditsKey, totalCredits);
        PlayerPrefs.Save();
    }

    public void AddEnemy(int enemyAmount)
    {
        currentEnemiesKilled += enemyAmount;
        totalEnemiesKilled += enemyAmount;

        // Save total enemies killed to PlayerPrefs
        PlayerPrefs.SetInt(totalPointsKey, totalEnemiesKilled);
        PlayerPrefs.Save();
    }

    public void PurchaseItem(ShopItem item)
    {
        // Subtract price from current credits
        totalCredits -= item.price;

        // Update PlayerPrefs or any other relevant logic to save the new total credits
        PlayerPrefs.SetInt(totalCreditsKey, totalCredits);

        SavePurchasedItem(item);

        // Check if the purchased item is related to health
        if (item.itemType == ShopItem.ItemType.Health)
        {
            IncreaseHealth(item.healthIncreaseAmount);
        }

        PlayerPrefs.Save();
    }

    void SavePurchasedItem(ShopItem item)
    {
        // Save purchased item data (e.g., item type, health increase amount, etc.) in PlayerPrefs or a custom save file
        PlayerPrefs.SetInt("PurchasedItem_Type", (int)item.itemType);
        PlayerPrefs.SetInt("PurchasedItem_HealthIncreaseAmount", item.healthIncreaseAmount);
    }

    void IncreaseHealth(int healthIncreaseAmount)
    {
        playerLoadout.baseHealth += healthIncreaseAmount;
    }
}
