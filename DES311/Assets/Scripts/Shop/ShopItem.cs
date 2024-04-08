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
        MoveSpeed,
    }
    public ItemType itemType;

    public string itemName;
    public string description;
    public int price;
    public Object item;
    public Sprite icon;

    public int healthIncreaseAmount;
 
}
