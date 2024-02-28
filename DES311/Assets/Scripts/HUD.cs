using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider levelSlider;
    public Slider healthSlider;
    public Player player;
    public PlayerMovement playerHealth;
    void Start()
    {
        if (levelSlider == null)
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        // Update values for the Slider
        UpdateLevelProgressBar();
    }

    void Update()
    {
        // Checks if the levelSlider or player reference is null
        if (levelSlider == null || player == null)
        {
            return;
        }

        // Update the Slider value
        UpdateLevelProgressBar();
        UpdateHealthBar();
    }

    void UpdateLevelProgressBar()
    {
        // Ensure player.requiredXP is not zero to avoid division by zero
        if (player.requiredXP != 0)
        {
            // Calculate the current progress as a percentage
            float progress = (float)player.currentXP / player.requiredXP;

            // Set the Slider value and maximum value
            levelSlider.value = progress;
            levelSlider.maxValue = 1f;
        }
    }

    public void UpdateHealthBar()
    {
        float currentHealth = playerHealth.currentWeapon.health / playerHealth.currentWeapon.healthMaxValue;
        healthSlider.value = currentHealth * playerHealth.currentWeapon.healthMaxValue;
        healthSlider.maxValue = playerHealth.currentWeapon.healthMaxValue;
    } 
}
