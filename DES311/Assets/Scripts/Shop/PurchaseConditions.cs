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

    void Start()
    {
        if (GameManager.instance != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        // Update current health upgrades
        currentHealthUpgrades.text = GameManager.instance.currentHealthUpgrades.ToString() + " / 5";

        // Update current credits text
        creditsText.text = GameManager.instance.totalCredits.ToString();

        // Get prices for the shop items
        int healthPrice = healthItem.price;
        int laserPrice = laserItem.price;
        int waterPrice = waterItem.price;

        // Check if the player has purchased the laser
        bool hasPurchasedLaser = PlayerPrefs.GetInt("HasPurchasedLaser", 0) == 1;
        currentLaserUpgrade.text = (hasPurchasedLaser ? "1" : "0") + " / 1";

        bool hasPurchasedWater = PlayerPrefs.GetInt("HasPurchasedWater", 0) == 1;
        currentWaterUpgrade.text = (hasPurchasedWater ? "1" : "0") + " / 1";

        // Grey out health button if max upgrades reached or not enough credits
        if (GameManager.instance.currentHealthUpgrades == 5 || GameManager.instance.totalCredits < healthPrice)
        {
            healthButton.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            healthButton.GetComponent<Image>().color = Color.white;
        }

        // Grey out laser button if laser is already purchased or player does not have enough credits
        if (hasPurchasedLaser || GameManager.instance.totalCredits < laserPrice)
        {
            laserButton.GetComponent<Image>().color = Color.grey;
            // If a laser is purchased, text is set to one and if not purchased text is set to 0
            currentLaserUpgrade.text = (hasPurchasedLaser ? "1" : "0") + " / 1";
        }
        else
        {
            laserButton.GetComponent<Image>().color = Color.white;
        }

        if (hasPurchasedWater || GameManager.instance.totalCredits < waterPrice)
        {
            waterButton.GetComponent<Image>().color = Color.grey;
            currentWaterUpgrade.text = (hasPurchasedWater ? "1" : "0") + " / 1";
        }
        else
        {
            waterButton.GetComponent<Image>().color = Color.white;
        }
    }
    public void UpdateUIAfterPurchase()
    {
        // Update the UI with the latest data
        UpdateUI();
    }
}
