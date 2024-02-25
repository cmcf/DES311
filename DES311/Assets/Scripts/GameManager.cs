using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    // Handles XP events
    public delegate void XPHandler(int amount);
    // Event is triggered when called from another script
    public event XPHandler XPEvent;

    public GameObject itemDisplayObject; // Reference to the game object with the ItemDisplay script

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only be one GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        Debug.Log("start");
        // Enable the game object temporarily to call the reset function
        itemDisplayObject.SetActive(true);

        // Call the reset weapon function
        itemDisplayObject.GetComponent<ItemDisplay>().ResetDefaultRifleUpgrades();

        // Disable the game object again if needed
        itemDisplayObject.SetActive(false);
    }

    public void IncreaseXP (int amount)
    {
        // If not null, increase XP by amoung gained
        XPEvent?.Invoke(amount);
    }
}
