using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    // Handles XP events
    public delegate void XPHandler(int amount);
    // Event is triggered when called from another script
    public event XPHandler XPEvent;

    public int currentEnemiesKilled;
    public int totalEnemiesKilled;

    public int currentCoins;
    public int totalCoins;

    private string totalPointsKey = "TotalPoints";
    string totalCoinsKey = "TotalCoins";

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
    }

    public void IncreaseXP (int amount)
    {
        // If not null, increase XP by amoung gained
        XPEvent?.Invoke(amount);
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public void AddCoins(int points)
    {
        currentCoins += points;
        totalCoins += points;

        // Save total coins  earned
        PlayerPrefs.SetInt(totalCoinsKey, totalCoins);
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

}
