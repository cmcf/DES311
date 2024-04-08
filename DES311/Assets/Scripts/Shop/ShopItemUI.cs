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

    [Header("Button")]
    [SerializeField] Button button;
    [SerializeField] Image greyButton;

    void Start()
    {
        Init(item, GameManager.instance.totalCredits);
    }


    public void Init(ShopItem item, int avaliableCredits)
    {
        this.item = item;

        itemName.text = item.itemName;
        price.text = "Cost: " + item.price.ToString() + " credits";
        description.text = item.description;

        Refresh(avaliableCredits);
    }

    void Refresh(int avaliableCredits)
    {
        if (avaliableCredits < item.price)
        {
            greyButton.enabled = true;
        }
        else
        {
            greyButton.enabled = false;
        }
    }

}
