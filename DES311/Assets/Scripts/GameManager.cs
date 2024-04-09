using UnityEngine;


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

    public int currentHealthUpgrades;

    string totalPointsKey = "TotalPoints";
    string totalCreditsKey = "TotalCoins";

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

        // Load the current number of health upgrades from PlayerPrefs
        currentHealthUpgrades = PlayerPrefs.GetInt("HealthUpgradesCount", 0);

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

        if (item.itemType == ShopItem.ItemType.Health)
        {

            // Check the current count of health upgrades
            currentHealthUpgrades = PlayerPrefs.GetInt("HealthUpgradesCount", 0);
            Debug.Log("Current Health Upgrades: " + currentHealthUpgrades);

            // If the current count is less than 5 and the player hasn't purchased this item yet, allow the purchase
            if (currentHealthUpgrades < 5)
            {
                Debug.Log("Can purchase health upgrade.");

                // Subtract price from current credits
                totalCredits -= item.price;

                // Update PlayerPrefs or any other relevant logic to save the new total credits
                PlayerPrefs.SetInt(totalCreditsKey, totalCredits);

                SavePurchasedItem(item);

                // Increase the count of health upgrades
                currentHealthUpgrades++;

                // Increase health
                IncreaseHealth(item.healthIncreaseAmount);

                // Update the UI
                ShopItemUI.FindObjectOfType<ShopItemUI>().UpdateUI();

                PlayerPrefs.Save();
            }
            else
            {
                // Update the UI
                ShopItemUI.FindObjectOfType<ShopItemUI>().UpdateUI();
                Debug.Log("Purchase failed: Maximum health upgrades reached!");
            }
        }
        else
        {
            // Subtract price from current credits
            totalCredits -= item.price;

            // Update PlayerPrefs
            PlayerPrefs.SetInt(totalCreditsKey, totalCredits);

            // Update the UI
            ShopItemUI.FindObjectOfType<ShopItemUI>().UpdateUI();

            SavePurchasedItem(item);

            PlayerPrefs.Save();
        }
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
