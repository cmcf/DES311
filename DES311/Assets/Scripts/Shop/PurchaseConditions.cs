using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseConditions : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button healthButton;
    [SerializeField] Button laserButton;
    [SerializeField] Button waterButton;

    [Header("Text Components")]
    [SerializeField] TextMeshProUGUI currentHealthUpgrades;
    [SerializeField] TextMeshProUGUI currentLaserUpgrade;
    [SerializeField] TextMeshProUGUI currentWaterUpgrade;
    [SerializeField] TextMeshProUGUI creditsText;

    [Header("Shop Items")]
    [SerializeField] ShopItem healthItem;
    [SerializeField] ShopItem laserItem;
    [SerializeField] ShopItem waterItem;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (gameManager == null) return;

        // Update current health upgrades
        currentHealthUpgrades.text = gameManager.gameData.currentHealthUpgrades.ToString() + " / 5";

        // Update current credits text
        creditsText.text = gameManager.gameData.totalCredits.ToString();

        // Get prices for the shop items
        int healthPrice = healthItem.price;
        int laserPrice = laserItem.price;
        int waterPrice = waterItem.price;

        // Check if the player has purchased the laser and water
        bool hasPurchasedLaser = gameManager.gameData.hasPurchasedLaser;
        bool hasPurchasedWater = gameManager.gameData.hasPurchasedWaterCard;

        currentLaserUpgrade.text = (hasPurchasedLaser ? "1" : "0") + " / 1";
        currentWaterUpgrade.text = (hasPurchasedWater ? "1" : "0") + " / 1";

        // Grey out health button if max upgrades reached or not enough credits
        if (gameManager.gameData.currentHealthUpgrades == 5 || gameManager.gameData.totalCredits < healthPrice)
        {
            healthButton.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            healthButton.GetComponent<Image>().color = Color.white;
        }

        // Grey out laser button if laser is already purchased or player does not have enough credits
        if (hasPurchasedLaser || gameManager.gameData.totalCredits < laserPrice)
        {
            laserButton.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            laserButton.GetComponent<Image>().color = Color.white;
        }

        // Grey out water button if water card is already purchased or player does not have enough credits
        if (hasPurchasedWater || gameManager.gameData.totalCredits < waterPrice)
        {
            waterButton.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            waterButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void PurchaseHealthUpgrade()
    {
        if (gameManager == null) return;

        if (gameManager.gameData.currentHealthUpgrades < 5 && gameManager.gameData.totalCredits >= healthItem.price)
        {
            gameManager.gameData.currentHealthUpgrades++;
            gameManager.gameData.totalCredits -= healthItem.price;
            UpdateUI();
            gameManager.SaveGameData();
        }
    }

    public void PurchaseLaser()
    {
        if (gameManager == null) return;

        if (!gameManager.gameData.hasPurchasedLaser && gameManager.gameData.totalCredits >= laserItem.price)
        {
            gameManager.gameData.hasPurchasedLaser = true;
            gameManager.gameData.totalCredits -= laserItem.price;
            UpdateUI();
            gameManager.SaveGameData();
        }
    }

    public void PurchaseWaterCard()
    {
        if (gameManager == null) return;

        if (!gameManager.gameData.hasPurchasedWaterCard && gameManager.gameData.totalCredits >= waterItem.price)
        {
            gameManager.gameData.hasPurchasedWaterCard = true;
            gameManager.gameData.totalCredits -= waterItem.price;
            UpdateUI();
            gameManager.SaveGameData();
        }
    }

    public void UpdateUIAfterPurchase()
    {
        UpdateUI();
    }
}
