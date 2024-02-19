using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Weapon Upgrade", menuName = "Weapon Item")]
public class WeaponItem : ScriptableObject
{
    public enum UpgradeType
    {
        FireRate,
        Cooldown,
        Speed,
        Default,
    }

    public UpgradeType modifiedAttribute;

    public float baseFireRate = 0.3f;
    public float baseCooldown = 1f;
    public float baseSpeed = 20f;

    // Upgrade amounts for each attribute
    public float fireRateIncrease;
    public float cooldownDecrease;
    public float speedIncrease;

    // Details
    public string itemName;
    public string description;

    public Sprite icon;
    public GameObject projectilePrefab;

}
