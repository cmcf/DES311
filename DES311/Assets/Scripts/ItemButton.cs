using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public WeaponItem upgrade; // Assign the upgrade to this field in the Inspector

    public void OnUpgradeButtonClick()
    {
        ItemDisplay upgradeManager = FindObjectOfType<ItemDisplay>();

        if (upgradeManager != null)
        {
            upgradeManager.ChosenUpgrade(upgrade);
        }
        else
        {
            Debug.LogWarning("UpgradeManager not found in the scene.");
        }
    }
}
