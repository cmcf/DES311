using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    PickupManager pickupManager;
    [SerializeField] float spinSpeed = 5f;

    void Update()
    {
        // Rotate the object around its up axis
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    void Start()
     {
        // Get reference to PickupManager instance
        pickupManager = GameManager.Instance.GetComponent<PickupManager>();

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
