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

    [Header("Weapon Base Values")]

    public float baseCooldown = 1f;
    public float baseSpeed = 20f;

    //Current values for weapon that can be modified
    
    public float cooldown;
    public float speed;

    // Upgrade amounts for each attribute
   
    public float cooldownDecrease;
    public float speedIncrease;

    // Upgrade details
    public string itemName;
    public string description;

    [Header("Weapon Maximum Values")]
    public float minFireRate = 0.2f;
    public float minCooldown = 0.1f;
    public float maxSpeed = 80f;

    public Sprite icon;
    public GameObject projectilePrefab;

}
