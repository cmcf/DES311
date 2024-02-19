using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public WeaponItem upgrade;
    public void OnUpgradeButtonClick()
    {
        ItemDisplay upgradeManager = FindObjectOfType<ItemDisplay>();

        if (upgradeManager != null)
        {
            upgradeManager.ChosenUpgrade(upgrade);
            // Resume the game
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogWarning("UpgradeManager not found in the scene.");
        }
    }
}
