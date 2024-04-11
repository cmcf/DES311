using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponItem;

[CreateAssetMenu(menuName = "Shop Item")]
public class ShopItem : ScriptableObject
{
    public enum ItemType
    {
        Health,
        Projectile,
        Card,
    }
    public ItemType itemType;

    public string itemName;
    [TextArea(3, 10)]
    public string description;
    public int price;
    public Object item;
    public GameObject projectile;
    public Sprite icon;

    public int healthIncreaseAmount;
 
}
