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

    [Header("Text Components")]
    [SerializeField] TextMeshProUGUI currentHealthUpgrades;
    [SerializeField] TextMeshProUGUI currentLaserUpgrade;
    [SerializeField] TextMeshProUGUI creditsText;

    [Header("Shop Items")]
    [SerializeField] ShopItem healthItem;
    [SerializeField] ShopItem laserItem;

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

        // Update credits
        creditsText.text = "Credits: " + GameManager.instance.totalCredits.ToString();

        // Get prices for health and laser upgrades
        int healthPrice = healthItem.price;
        int laserPrice = laserItem.price;

        // Check if the player has purchased the laser
        bool hasPurchasedLaser = PlayerPrefs.GetInt("HasPurchasedLaser", 0) == 1;
        currentLaserUpgrade.text = (hasPurchasedLaser ? "1" : "0") + " / 1";

        // Grey out health button if max upgrades reached or not enough credits
        if (GameManager.instance.currentHealthUpgrades == 5 || GameManager.instance.totalCredits < healthPrice)
        {
            healthButton.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            healthButton.GetComponent<Image>().color = Color.white;
        }

        // Grey out laser button if laser is purchased or not enough credits
        if (hasPurchasedLaser || GameManager.instance.totalCredits < laserPrice)
        {
            laserButton.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            laserButton.GetComponent<Image>().color = Color.white;
        }
    }

    // Method to update the UI after a purchase
    public void UpdateUIAfterPurchase()
    {
        // Update the UI with the latest data
        UpdateUI();
    }
}
