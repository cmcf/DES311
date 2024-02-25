using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public WeaponItem upgrade;
    public ItemManager itemManager;

    ItemDisplay upgradeManager;
    void Start()
    { 
        upgradeManager = FindObjectOfType<ItemDisplay>();
    }
    public void OnUpgradeButtonClick()
    {
        if (upgradeManager != null)
        {
            upgradeManager.ChosenUpgrade(upgrade);
            
            // Resume the game
            Time.timeScale = 1f;

            if (itemManager != null)
            {
                itemManager.HideItemSelection();
            }

        }
    }
}
