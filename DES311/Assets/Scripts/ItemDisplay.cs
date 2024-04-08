using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static WeaponItem;
using NUnit.Framework.Internal;

public class ItemDisplay : MonoBehaviour
{
    public PlayerMovement playerScript;
    public Player playerHealth;

    [Header("Text Components")]
    public TextMeshProUGUI description;
    public TextMeshProUGUI weaponName;

    public WeaponItem weaponUpgrade;
    WeaponItem currentWeapon;

    void Start()
    {
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
            currentWeapon.fireRate -= upgrade.cooldownDecrease;
            Debug.Log(playerScript.currentLoadout.baseFireRate);
            break;
        case WeaponItem.UpgradeType.Speed:
            currentWeapon.speed += upgrade.speedIncrease;
            Debug.Log(playerScript.currentLoadout.baseSpeed);
            break;
         case WeaponItem.UpgradeType.MoveSpeed:
              currentWeapon.moveSpeed += upgrade.movementSpeedIncrease;
              Debug.Log(playerScript.currentLoadout.baseMoveSpeed);
              break;
            case WeaponItem.UpgradeType.Health:
                currentWeapon.healthMaxValue += upgrade.healthIncrease;
                currentWeapon.health += upgrade.healthIncrease;
                break;
            case WeaponItem.UpgradeType.Bullet:
                currentWeapon.projectilePrefab = upgrade.projectileUpgrade;
                break;

        }

    }

}
