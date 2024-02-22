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

        }
        Debug.Log("Applied upgrade: " + upgrade.name);
        HideItemSelection();
    }

    public void ResetDefaultRifleUpgrades()
    {
        // Reset default rifle upgrades to their base values
        playerScript.currentWeapon.cooldown = playerScript.currentWeapon.baseCooldown;
        playerScript.currentWeapon.speed = playerScript.currentWeapon.baseSpeed;
        playerScript.currentWeapon.moveSpeed = playerScript.currentWeapon.baseMoveSpeed;
    }

}
