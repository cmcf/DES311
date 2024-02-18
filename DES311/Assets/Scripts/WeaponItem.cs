using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Upgrade", menuName = "Weapon Item")]
public class WeaponItem : ScriptableObject
{
    public string itemName;
    public string description;

    public Sprite icon;
    public GameObject projectilePrefab;

    public float fireRate;
    public float cooldown;
    public float speed;

}
