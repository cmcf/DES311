using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public ShopItem item;
    [SerializeField] Image icon;

    [Header("Text Components")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI price;
    public TextMeshProUGUI description;

    void Start()
    {
        if (GameManager.instance != null)
        {
            Init(item, GameManager.instance.totalCredits);
        }  
    }

    public void Init(ShopItem item, int avaliableCredits)
    {
        this.item = item;

        itemName.text = item.itemName;
        price.text = "Cost: " + item.price.ToString() + " credits";
        description.text = item.description;

    }

}
