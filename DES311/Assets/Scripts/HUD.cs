using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider levelSlider;
    public Player player;
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

        // Set initial values for the Slider
        UpdateLevelProgressBar();
    }

    void Update()
    {
        // Check if the levelSlider or player reference is null
        if (levelSlider == null || player == null)
        {
            return;
        }

        // Update the Slider value
        UpdateLevelProgressBar();
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
}
