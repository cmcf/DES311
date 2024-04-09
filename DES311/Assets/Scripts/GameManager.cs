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
    public bool hasPurchasedLaser = false;

    string totalPointsKey = "TotalPoints";
    string totalCreditsKey = "TotalCoins";

    string healthUpgradesCountKey = "HealthUpgradesCount";
    string hasPurchasedLaserKey = "HasPurchasedLaser";

    [SerializeField] int totalHealthUpgraded;

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;

            // Set this GameObject to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
            
        }
        else if (instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only be one GameManager.
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //playerLoadout.baseHealth = 50;
        // Load total points from PlayerPrefs
        totalEnemiesKilled = PlayerPrefs.GetInt(totalPointsKey, 0);
        totalCredits = PlayerPrefs.GetInt(totalCreditsKey, 0);

        // Load the current number of health upgrades from PlayerPrefs
        currentHealthUpgrades = PlayerPrefs.GetInt("HealthUpgradesCount", 0);

        // Load the purchased laser status from PlayerPrefs
        hasPurchasedLaser = PlayerPrefs.GetInt(hasPurchasedLaserKey, 0) == 1;


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
            currentHealthUpgrades = PlayerPrefs.GetInt(healthUpgradesCountKey, 0);
            Debug.Log("Current Health Upgrades: " + currentHealthUpgrades);

            // If the current count is less than 5 and the player hasn't purchased this item yet, allow the purchase
            if (currentHealthUpgrades < 5)
            {

                // Subtract price from current credits
                totalCredits -= item.price;

                // Update PlayerPrefs to save the new total credits
                PlayerPrefs.SetInt(totalCreditsKey, totalCredits);

                // Save the purchased health upgrades count
                currentHealthUpgrades++;

                // Save the purchased health upgrades count to PlayerPrefs
                PlayerPrefs.SetInt(healthUpgradesCountKey, currentHealthUpgrades);

                // Increase health
                IncreaseHealth(item.healthIncreaseAmount);

                // Update the UI
                PurchaseConditions.FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();

                PlayerPrefs.Save();
            }
            else
            {
                PurchaseConditions.FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();
                Debug.Log("Purchase failed: Maximum health upgrades reached!");
            }
        }

        else if (item.itemType == ShopItem.ItemType.Projectile)
             {
                if (hasPurchasedLaser)
                {
                    // Player has already purchased the laser, do not allow purchasing again
                    return;
                }

                // Check if player has enough credits
                if (totalCredits >= item.price)
                {
                    // Deduct the price from total credits
                    totalCredits -= item.price;

                    PurchaseConditions.FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();

                    // Update PlayerPrefs or any other relevant logic to save the new total credits
                    PlayerPrefs.SetInt(totalCreditsKey, totalCredits);

                    // Update the player's loadout with the purchased laser
                    UpdateStartingProjectile(item);

                    // Set hasPurchasedLaser to true
                    hasPurchasedLaser = true;

                    // Save the hasPurchasedLaser status
                    PlayerPrefs.SetInt("HasPurchasedLaser", hasPurchasedLaser ? 1 : 0);
                    PlayerPrefs.Save();

                    // Save purchased item data
                    SavePurchasedItem(item);
                }
                else
                {
                    PurchaseConditions.FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();
                    Debug.Log("Not enough credits");
                }
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
        totalHealthUpgraded = healthIncreaseAmount;
    }
    void UpdateStartingProjectile(ShopItem item)
    {
        // Update the player's default projectile to the purchased item's projectile
        playerLoadout.defaultProjectile = item.projectile;

        // Save that the player has purchased the laser
        PlayerPrefs.SetInt("HasPurchasedLaser", 1);
        PlayerPrefs.Save();

    }

}
