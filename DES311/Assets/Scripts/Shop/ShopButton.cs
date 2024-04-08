using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public ShopItem item;

    public void OnPurchaseButtonClicked()
    {

        // Check if player has enough credits
        if (GameManager.instance.totalCredits >= item.price)
        {
            Debug.Log("Shop item purchased: " + item.itemName);
            GameManager.instance.PurchaseItem(item);
        }
        else
        {
            Debug.Log("Purchase failed: Insufficient credits!");
        }
    }
}
