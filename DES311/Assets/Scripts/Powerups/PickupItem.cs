using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    PickupManager pickupManager;

     void Start()
     {
        // Get reference to PickupManager instance
        pickupManager = GameManager.instance.GetComponent<PickupManager>();

        if (pickupManager == null)
        {
            Debug.LogError("PickupManager is null!");
        }
     }
    void OnTriggerEnter(Collider other)
     {
        if (other.gameObject.CompareTag("Player"))
        {
            if (pickupManager != null)
            {
                pickupManager.ActivatePowerUp();
            }

            Destroy(gameObject);
        }
    }

}
