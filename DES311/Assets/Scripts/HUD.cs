using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Player player;
    public PlayerMovement playerHealth;

    [Header("Sliders")]
    public Slider levelSlider;
    public Slider healthSlider;

    [Header("Text")]
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI timerText;

    float totalTime = 300f;
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
        StartCoroutine(StartTimer());
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
        // Updates health slider values
        float currentHealth = playerHealth.currentLoadout.health / playerHealth.currentLoadout.healthMaxValue;
        healthSlider.value = currentHealth * playerHealth.currentLoadout.healthMaxValue;
        healthSlider.maxValue = playerHealth.currentLoadout.healthMaxValue;
        // Update health text values
        currentHealthText.text = playerHealth.currentLoadout.health.ToString() + "/";
        maxHealthText.text = playerHealth.currentLoadout.healthMaxValue.ToString();
        
    }

    IEnumerator StartTimer()
    {
        float remainingTime = totalTime;

        while (remainingTime > 0)
        {
            // Convert remaining time to minutes and seconds
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Update timer text
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrease remaining time by 1 second
            remainingTime -= 1f;
        }

        // Timer reaches zero, display 00:00
        timerText.text = "00:00";

        player.DisablePlayerMovement();
        StartCoroutine(LoadWinScene(1f));
    }

    IEnumerator LoadWinScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the enemy object
        Destroy(gameObject);
        GameManager.instance.LevelComplete();

        Time.timeScale = 0f;
    }
}
