using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public ShopItem item;

    [Header("Text Components")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI price;
    public TextMeshProUGUI description;

    void Start()
    {
        if (GameManager.instance != null)
        {
            Init(item, GameManager.instance.gameData.totalCredits);
        }  
    }

    public void Init(ShopItem item, int avaliableCredits)
    {
        // Displays assigned item values
        this.item = item;

        itemName.text = item.itemName;
        price.text = item.price.ToString();
        description.text = item.description;

    }

}
