using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade Item")]
public class WeaponItem : ScriptableObject
{
    public enum UpgradeType
    {
        Cooldown,
        Speed,
        Bullet,
        Default,
        Health,
        MoveSpeed,
    }

    [Header("Details")]
    public string itemName;
    public string description;

    public Sprite icon;
    public GameObject projectilePrefab;

    public UpgradeType modifiedAttribute;

    [Header("Default Values")]
    public float baseCooldown = 1f;
    public float baseSpeed = 20f;
    public float baseMoveSpeed = 3.5f;
    public float baseHealth = 50f; 

    [Header("Current Values")]
    public float cooldown;
    public float speed;
    public float moveSpeed;
    public float health;
    public float healthMaxValue = 50f;

    [Header("Upgrade Values")]
    public float cooldownDecrease;
    public float speedIncrease;
    public float movementSpeedIncrease;
    public float healthIncrease;

    [Header("Max Upgrade Values")]
    public float minCooldown = 0.2f;
    public float maxSpeed = 95f;
    public float maxMoveSpeed = 8.5f;
    public float healthUpgradeMax = 90f;
    

}
