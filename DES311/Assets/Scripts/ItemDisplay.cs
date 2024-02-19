using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static WeaponItem;

public class ItemDisplay : MonoBehaviour
{
    public PlayerMovement playerScript;

    [Header("Text Components")]
    public TextMeshProUGUI description;
    public TextMeshProUGUI cooldownDecrease;
    public TextMeshProUGUI weaponName;

    public WeaponItem weaponUpgrade;
    WeaponItem currentWeapon;

    void Start()
    {
        currentWeapon = playerScript.GetDefaultWeapon();
        weaponName.text = weaponUpgrade.itemName;
        description.text = weaponUpgrade.description;
    }

    public void HideItemSelection()
    {
        gameObject.SetActive(false);
    }

    public void ChosenUpgrade(WeaponItem upgrade)
    {
  // Apply modifications based on upgrade attributes to the default weapon
    switch (upgrade.modifiedAttribute)
    {
        case WeaponItem.UpgradeType.FireRate:
            playerScript.currentWeapon.baseFireRate += upgrade.fireRateIncrease;
            break;
        case WeaponItem.UpgradeType.Cooldown:
            playerScript.currentWeapon.baseCooldown -= upgrade.cooldownDecrease;
            Debug.Log(playerScript.currentWeapon.baseCooldown);
            break;
        case WeaponItem.UpgradeType.Speed:
            playerScript.currentWeapon.baseSpeed += upgrade.speedIncrease;
            Debug.Log(playerScript.currentWeapon.baseSpeed);
            break;
        // Add more cases for additional attributes
    }

    // Log the name of the applied upgrade
    Debug.Log("Applied upgrade: " + upgrade.name);
        HideItemSelection();
    }
}
