using UnityEngine;
using System.IO;
using System.Xml.Schema;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public WeaponItem playerLoadout;
    PlayerMovement playerStats;
    ShopItem item;
    [SerializeField] Points pointsScript;
    [SerializeField] Canvas winCanvas;

    // Handles XP events
    public delegate void XPHandler(int amount);
    // Event is triggered when called from another script
    public event XPHandler XPEvent;

    // Define a data structure to hold the game data
    [System.Serializable]
    public class GameData
    {
        public int totalCredits;
        public int currentHealthUpgrades;
        public bool hasPurchasedLaser;
        public bool hasPurchasedWaterCard;
    }
    public int currentCredits;

    public int currentEnemiesKilled;

    public GameData gameData;

    private string savePath;

    void Awake()
    {
        // Check if instance already exists
        if (Instance == null)
        {
            // If not, set instance to this
            Instance = this;

            // Set this GameObject to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            savePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        }
        else if (Instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only be one GameManager.
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load game data
        LoadGameData();

        //ResetGameData();
       // PlayerPrefs.DeleteAll();

        // Disable level complete canvas at start of level 
        if (winCanvas != null)
        {
            winCanvas.enabled = false;
        }
    }

    void LoadGameData()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            // Initialize default values if the save file doesn't exist
            gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(savePath, jsonData);
    }

    public void IncreaseXP(int amount)
    {
        // If not null, increase XP by amount gained
        XPEvent?.Invoke(amount);
    }

    public void AddCredits(int currentLevel)
    {
        int creditsToAdd = CalculateCreditsToAdd(currentLevel);

        // Update the current credits earned
        currentCredits += creditsToAdd;

        // Update the total credits earned
        gameData.totalCredits += creditsToAdd;
    }

    int CalculateCreditsToAdd(int currentLevel)
    {
        // Calculate the amount of credits to add based on the player's current level
        if (currentLevel <= 3)
        {
            return 5;
        }
        else if (currentLevel <= 6)
        {
            return 10;
        }
        else
        {
            return 20;
        }
    }

    public void AddRewardCredits()
    {
        Debug.Log("Add reward credits");
        gameData.totalCredits += 20;
    }

    public void PurchaseItem(ShopItem item)
    {
        switch (item.itemType)
        {
            case ShopItem.ItemType.Health:
                PurchaseHealthUpgrade(item);
                break;
            case ShopItem.ItemType.Projectile:
                PurchaseProjectile(item, gameData.hasPurchasedLaser, "HasPurchasedLaser");
                break;
            case ShopItem.ItemType.Card:
                PurchaseWaterCard(item, gameData.hasPurchasedWaterCard, "HasPurchasedWater");
                break;
            default:
                Debug.LogError("Invalid item type: " + item.itemType);
                break;
        }
    }


    public void AddEnemy(int enemyAmount)
    {
        currentEnemiesKilled += enemyAmount;
    }

    public void PurchaseHealthUpgrade(ShopItem item)
    {
        // Check if the player has enough credits to purchase the health upgrade
        if (PurchaseItemWithCredits(item.price))
        {
            // Increment health upgrades count
            gameData.currentHealthUpgrades++;

            // Increase health based on the item's health increase amount
            IncreaseHealth(item.healthIncreaseAmount);

            // Save health upgrade information to PlayerPrefs
            PlayerPrefs.SetInt("HealthUpgradesCount", gameData.currentHealthUpgrades);
            PlayerPrefs.SetInt("HealthIncreaseAmount", item.healthIncreaseAmount);
            PlayerPrefs.Save();

            // Save game data
            SaveGameData();

            // Update UI after purchase
            FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();

        }
        else
        {
            Debug.Log("Not enough credits to purchase the item");
        }
    }

    public void PurchaseProjectile(ShopItem item, bool hasPurchasedItem, string playerPrefsKey)
    {
        if (hasPurchasedItem)
        {
            // Do not complete purchase if player already has laser
            return;
        }

        if (PurchaseItemWithCredits(item.price))
        {
            hasPurchasedItem = true;
            gameData.hasPurchasedLaser = true;

            SaveGameData();
            UpdateStartingProjectile(item);
            SavePurchasedItem(item);
            FindObjectOfType<PurchaseConditions>().UpdateUIAfterPurchase();
        }
        else
        {
            Debug.Log("Not enough credits to purchase the item");
        }
    }

    public void PurchaseWaterCard(ShopItem item, bool hasPurchasedItem, string playerPrefsKey)
    {
        if (hasPurchasedItem)
        {
            return;
        }

        if (PurchaseItemWithCredits(item.price))
        {
            hasPurchasedItem = true;
            gameData.hasPurchasedWaterCard = true;
            SaveGameData();
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
        // Check if the player has enough credits to make the purchase
        if (gameData.totalCredits >= price)
        {
            // Deduct the price from the total credits
            gameData.totalCredits -= price;

            // Save the updated credits to the game data
            SaveGameData();

            // Return true to indicate the successful purchase
            return true;
        }
        else
        {
            // Return false to indicate that the purchase failed due to insufficient credits
            return false;
        }
    }

    void SavePurchasedItem(ShopItem item)
    {
        // Save purchased item data
    }

    void IncreaseHealth(int healthIncreaseAmount)
    {
        playerLoadout.baseHealth += healthIncreaseAmount;
    }

    void UpdateStartingProjectile(ShopItem item)
    {
        // Update the player's default projectile to the purchased item's projectile
        playerLoadout.projectilePrefab = item.projectile;
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

    public void ResetGameData()
    {
        // Reset all game data to their default values
        gameData.totalCredits = 0;
        gameData.currentHealthUpgrades = 0;
        gameData.hasPurchasedLaser = false;
        gameData.hasPurchasedWaterCard = false;

        // Save the reset game data
        SaveGameData();
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
