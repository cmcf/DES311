using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static WeaponItem;

public class ItemDisplay : MonoBehaviour
{
    public PlayerMovement playerScript;
    public Player playerHealth;
    [Header("Text Components")]
    public TextMeshProUGUI description;
    public TextMeshProUGUI cooldownDecrease;
    public TextMeshProUGUI weaponName;

    public WeaponItem weaponUpgrade;
    WeaponItem currentWeapon;

    void Start()
    {
        if (currentWeapon ==null) { return; }
        currentWeapon = playerScript.GetDefaultWeapon();
        weaponName.text = weaponUpgrade.itemName;
        description.text = weaponUpgrade.description;
    }

    public void ChosenUpgrade(WeaponItem upgrade)
    {
  // Apply modifications based on upgrade attributes to the default weapon
    switch (upgrade.modifiedAttribute)
    {
        case WeaponItem.UpgradeType.Cooldown:
            currentWeapon.cooldown -= upgrade.cooldownDecrease;
            Debug.Log(playerScript.currentWeapon.baseCooldown);
            break;
        case WeaponItem.UpgradeType.Speed:
            currentWeapon.speed += upgrade.speedIncrease;
            Debug.Log(playerScript.currentWeapon.baseSpeed);
            break;
         case WeaponItem.UpgradeType.MoveSpeed:
              currentWeapon.moveSpeed += upgrade.movementSpeedIncrease;
              Debug.Log(playerScript.currentWeapon.baseMoveSpeed);
              break;
            case WeaponItem.UpgradeType.Health:
                currentWeapon.healthMaxValue += upgrade.healthIncrease;
                currentWeapon.health += upgrade.healthIncrease;
                break;
             

        }
        Debug.Log("Applied upgrade: " + upgrade.name);
    }

    public void ResetDefaultRifleUpgrades()
    {
        if (currentWeapon == null) { return; }
        // Reset default rifle upgrades to their base values
        playerScript.currentWeapon.cooldown = playerScript.currentWeapon.baseCooldown;
        playerScript.currentWeapon.speed = playerScript.currentWeapon.baseSpeed;
        playerScript.currentWeapon.moveSpeed = playerScript.currentWeapon.baseMoveSpeed;
        playerScript.currentWeapon.healthMaxValue = playerScript.currentWeapon.baseHealth;
    }

}
