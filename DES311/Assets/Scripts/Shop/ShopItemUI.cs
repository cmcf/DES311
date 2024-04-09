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
    [SerializeField] TextMeshProUGUI currentUpgradeText;
    [SerializeField] TextMeshProUGUI creditsText;

    [Header("Button")]
    [SerializeField] Button button;

    void Start()
    {
        if (GameManager.instance != null)
        {
            Init(item, GameManager.instance.totalCredits);
            creditsText.text = creditsText.text = "Credits: " + GameManager.instance.totalCredits.ToString();
        }  
    }

    public void Init(ShopItem item, int avaliableCredits)
    {
        this.item = item;

        itemName.text = item.itemName;
        price.text = "Cost: " + item.price.ToString() + " credits";
        description.text = item.description;
        currentUpgradeText.text = GameManager.instance.currentHealthUpgrades.ToString() + " / 5";
        if (GameManager.instance.currentHealthUpgrades == 5)
        {
            button.GetComponent<Image>().color = Color.grey;
        }
        Refresh(avaliableCredits);
    }

    void Refresh(int avaliableCredits)
    {
        // Button is greyed out if player does not have enough credits
        if (avaliableCredits < item.price)
        {
            button.GetComponent<Image>().color = Color.grey;
        }
        else if (avaliableCredits > item.price && GameManager.instance.currentHealthUpgrades < 5)

        {
            button.GetComponent<Image>().color = Color.white;
        }
    }

    public void UpdateUI()
    {
        currentUpgradeText.text = GameManager.instance.currentHealthUpgrades.ToString() + " / 5";
        creditsText.text = "Credits: " + GameManager.instance.totalCredits.ToString();

        if (GameManager.instance.currentHealthUpgrades == 5 || GameManager.instance.currentCredits <= item.price)
        {
            button.GetComponent<Image>().color = Color.grey;
        }
    }

}
