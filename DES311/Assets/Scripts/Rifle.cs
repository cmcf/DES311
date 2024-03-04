using UnityEngine;

public class Rifle : Weapon
{
    PlayerMovement playerScript;
    Player playerStats;
    public GameObject muzzle;

    bool playMuzzle;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find and store the PlayerMovement script in the scene
        playerScript = FindObjectOfType<PlayerMovement>();
        playerStats = FindObjectOfType<Player>();
    }

    void Update()
    {
        // Check if the playerScript is assigned
        if (playerScript != null)
        {
            if (playerStats.isDead) { return; }
            // Activate the muzzle flash GameObject if the player is aiming and it's not already active
            if (playerScript.isFiring)
            {
                muzzle.SetActive(true);
            }
            // Deactivate the muzzle flash GameObject if the player is not aiming
            else
            {
                muzzle.SetActive(false);
            }
        }
    }

}

