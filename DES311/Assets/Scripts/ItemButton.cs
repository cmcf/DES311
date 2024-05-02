using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public WeaponItem upgrade;
    public ItemManager itemManager;
    Player playerScript;

    UpgradeSelection upgradeManager;

    bool hasSelectedCard;
    void Start()
    { 
        upgradeManager = FindObjectOfType<UpgradeSelection>();
        playerScript = FindObjectOfType<Player>();
        hasSelectedCard = false;
    }
    public void OnUpgradeButtonClick()
    {
        if (!hasSelectedCard)
        {
            if (upgradeManager != null)
            {
                upgradeManager.ChosenUpgrade(upgrade);

                // Set the flag to true to indicate that an upgrade has been chosen
                hasSelectedCard = true;

                // Log the upgrade selection
                Debug.Log("Upgrade chosen: " + upgrade.itemName);

                // Resume the game
                Time.timeScale = 1f;

                playerScript.EnablePlayerMovement();
                
                hasSelectedCard = false;

                // Hides the upgrade cards if the are present
                if (itemManager != null)
                {
                    itemManager.HideItemSelection();
                }
               
            }
        }

        else
        {
            Debug.Log("Upgrade already chosen. Cannot choose another upgrade.");
        }
    }


}
