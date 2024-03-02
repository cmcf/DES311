using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSelection : MonoBehaviour
{
    // Reference to the PowerUp script
    public PickupManager pickupManager;

    // Method to apply the power-up when the card is clicked
    public void ApplyPowerUp()
    {
        // Call the ActivatePowerUp method of the PowerUp script
        pickupManager.ActivatePowerUp();

        // Disable the card object
        gameObject.SetActive(false);
    }
}
