using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System")]
public class Shop : ScriptableObject
{
    [SerializeField] ShopItem[] shopItems;

    public ShopItem[] GetShopItems() { return shopItems; }

    public bool TryPurchase(ShopItem selectedItem, Credits purchaser)
    {
        return purchaser.Purchase(selectedItem.price, selectedItem.item);
    }
}