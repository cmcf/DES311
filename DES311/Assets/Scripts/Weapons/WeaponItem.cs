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

    [Header("Details")]
    public string itemName;
    public string description;

    public Sprite icon;
    public GameObject projectilePrefab;

    public UpgradeType modifiedAttribute;

    [Header("Default Values")]
    public float baseFireRate = 0.3f;
    public float baseCooldown = 1f;
    public float baseSpeed = 20f;

    [Header("Current Values")]
    public float fireRate;
    public float cooldown;
    public float speed;

    [Header("Upgrade Values")]
    public float fireRateIncrease;
    public float cooldownDecrease;
    public float speedIncrease;

}
