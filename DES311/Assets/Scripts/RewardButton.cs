using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class RewardsData
{
    public DateTime lastAccessTime;
}

public class RewardButton : MonoBehaviour
{
    public Button rewardButton;
 

    const string LastAccessKey = "LastRewardsAccess";
    TimeSpan disableDuration = TimeSpan.FromHours(24);

    void Start()
    {
        // Check if the button should be enabled or disabled on start
        CheckRewardsButtonStatus();
    }

    public void OnRewardsButtonClicked()
    {
        // Show Ad
        AdManager.Instance.ShowAd();

        // Update last access time
        PlayerPrefs.SetString(LastAccessKey, DateTime.Now.ToString());
        PlayerPrefs.Save();

        // Disable the reward button
        DisableButton();

        // Start coroutine to enable the button after the disable duration
        StartCoroutine(EnableButtonAfterDelay());
    }

    void CheckRewardsButtonStatus()
    {
        // Checks if last access time is recorded
        if (PlayerPrefs.HasKey(LastAccessKey))
        {
            // Get the last access time
            DateTime lastAccessTime = DateTime.Parse(PlayerPrefs.GetString(LastAccessKey));

            // Calculates the time difference between now and last access time
            TimeSpan timeSinceLastAccess = DateTime.Now - lastAccessTime;

            // Checks if enough time has passed to enable the button again
            if (timeSinceLastAccess >= disableDuration)
            {
                // Enable the button
                SetButtonActive();
            }
            else
            {
                // Disable the rewards button
                DisableButton();

                // Start coroutine to enable the button after the remaining time
                StartCoroutine(EnableButtonAfterDelay(disableDuration - timeSinceLastAccess));
            }
        }
        else
        {
            // Enable the button if it's the first time accessing it
            SetButtonActive();
        }
    }

    void SetButtonActive()
    {
        rewardButton.interactable = true;
        rewardButton.GetComponent<Image>().color = Color.white;
    }

    void DisableButton()
    {
        rewardButton.interactable = false;
        rewardButton.GetComponent<Image>().color = Color.grey;
    }

    IEnumerator EnableButtonAfterDelay(TimeSpan delay)
    {
        yield return new WaitForSeconds((float)delay.TotalSeconds);

        // Enable the button after the delay
        SetButtonActive();
    }

    IEnumerator EnableButtonAfterDelay()
    {
        yield return new WaitForSeconds((float)disableDuration.TotalSeconds);

        // Enable the button after the delay
        SetButtonActive();
    }
}