using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public WeaponItem playerLoadout;
    PlayerMovement playerStats;
    ShopItem item;
    [SerializeField] Points pointsScript;
    [SerializeField] Canvas winCanvas;
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
    public bool hasPurchasedWaterCard = false;

    string totalPointsKey = "TotalPoints";
    string totalCreditsKey = "TotalCoins";

    string healthUpgradesCountKey = "HealthUpgradesCount";
    string hasPurchasedLaserKey = "HasPurchasedLaser";
    string hasPurchasedWaterKey = "HasPurchasedWater";

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
        // Load the purchased laser status
        hasPurchasedLaser = PlayerPrefs.GetInt(hasPurchasedLaserKey, 0) == 1;
        // Laod the purchased water status
        hasPurchasedWaterCard = PlayerPrefs.GetInt(hasPurchasedWaterKey, 0) == 1;

        if (winCanvas!= null)
        {
            winCanvas.enabled = false;  
        }


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
        switch (item.itemType)
        {
            case ShopItem.ItemType.Health:
                PurchaseHealthUpgrade(item);
                break;
            case ShopItem.ItemType.Projectile:
                PurchaseProjectile(item, hasPurchasedLaser, "HasPurchasedLaser");
                break;
            case ShopItem.ItemType.Card:
                PurchaseWaterCard(item, hasPurchasedWaterCard, "HasPurchasedWater");
                break;
            default:
                Debug.LogError("Invalid item type: " + item.itemType);
                break;
        }
    }

    void PurchaseHealthUpgrade(ShopItem item)
    {
        // Check the current count of health upgrades
        currentHealthUpgrades = PlayerPrefs.GetInt(healthUpgradesCountKey, 0);

        // If the current count is less than 5 and the player hasn't purchased this item yet, allow the purchase
        if (currentHealthUpgrades < 5)
        {
            if (PurchaseItemWithCredits(item.price))
            {
                currentHealthUpgrades++;
                IncreaseHealth(item.healthIncreaseAmount);
                PlayerPrefs.SetInt(healthUpgradesCountKey, currentHealthUpgrades);
                PlayerPrefs.Save();
                FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();
            }
        }
        else
        {
            Debug.Log("Max health upgrades reached");
        }
    }

    void PurchaseProjectile(ShopItem item, bool hasPurchasedItem, string playerPrefsKey)
    {
        if (hasPurchasedItem)
        {
            // Do not complete purchase if player already has laser
            return;
        }

        if (PurchaseItemWithCredits(item.price))
        {
            hasPurchasedItem = true;
            PlayerPrefs.SetInt(playerPrefsKey, hasPurchasedItem ? 1 : 0);
            PlayerPrefs.Save();
            UpdateStartingProjectile(item);
            SavePurchasedItem(item);
            FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();
        }
        else
        {
            Debug.Log("Not enough credits to purchase the item");
        }
    }

    void PurchaseWaterCard(ShopItem item, bool hasPurchasedItem, string playerPrefsKey)
    {
        if (hasPurchasedItem)
        {
            return;
        }

        if (PurchaseItemWithCredits(item.price))
        {
            hasPurchasedItem = true;
            PlayerPrefs.SetInt(playerPrefsKey, hasPurchasedItem ? 1 : 0);
            PlayerPrefs.Save();
            SavePurchasedItem(item);
            FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();
        }
        else
        {
            Debug.Log("Not enough credits to purchase the item");
        }
    }

    bool PurchaseItemWithCredits(int price)
    {
        if (totalCredits >= price)
        {
            totalCredits -= price;
            PlayerPrefs.SetInt(totalCreditsKey, totalCredits);
            return true;
        }
        return false;
    }

    void SavePurchasedItem(ShopItem item)
    {
       // Save purchased item data
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

    public void LevelComplete()
    {
        if (pointsScript != null)
        {
            pointsScript.UpdatePointsText();
        }
        else
        {
            Debug.Log("Null");
        }

        // Load the win screen scene
        winCanvas.enabled = true;
    }

}
