using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    public PlayerMovement playerScript;

    [Header("Text Components")]
    public TextMeshProUGUI projectileSpeedIncrease;
    public TextMeshProUGUI weaponChoice;


    public WeaponItem weaponItem;
    public WeaponItem currentWeapon;

    void Start()
    {
        gameObject.SetActive(false);
        playerScript.GetDefaultWeapon();
    }

    public void DisplayItems()
    {
        weaponChoice.text = weaponItem.itemName;
        float currentWeaponSpeed = playerScript.currentWeapon.speed;
        float projectileSpeedDifference = weaponItem.speed - currentWeaponSpeed;
        projectileSpeedIncrease.text = "+ " + projectileSpeedDifference.ToString() + " projectile speed";
        gameObject.SetActive(true);

    }

    public void HideItemSelction()
    {
        gameObject.SetActive(false);
    }

    public void ChosenUpgrade(WeaponItem upgrade)
    {
       
        // Check if the upgrade has a prefab
        if (upgrade.projectilePrefab == null)
        {
            Debug.LogWarning("Upgrade prefab is null.");
            return;
        }

        // Find the player object in the scene
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

            // Check if the PlayerMovement script was found
            if (playerMovement != null)
            {
                // Set the current weapon of the PlayerMovement script to the chosen upgrade
                playerMovement.currentWeapon = upgrade;
                Debug.Log(upgrade.name);
            }
        }

        HideItemSelction();
    }
}