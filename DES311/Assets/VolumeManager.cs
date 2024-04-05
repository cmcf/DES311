using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Button musicToggleButton;
    public Button sfxToggleButton;

    private bool musicEnabled = true;
    private bool sfxEnabled = true;

    void Start()
    {
        // Initialize buttons' states
        UpdateMusicButton();
        UpdateSFXButton();
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        UpdateMusicButton();

        // Perform additional actions if needed
        if (musicEnabled)
        {
            // Code to enable music
        }
        else
        {
            // Code to disable music
        }
    }

    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;
        UpdateSFXButton();

        if (sfxEnabled)
        {
            UpdateSFXButton();

            AudioManager.instance.ToggleSFX(sfxEnabled);
        }
        else
        {
            // Code to disable SFX
        }
    }

    private void UpdateMusicButton()
    {
        if (musicToggleButton != null)
        {
            musicToggleButton.GetComponentInChildren<Text>().text = musicEnabled ? "Music: On" : "Music: Off";
        }
    }

    private void UpdateSFXButton()
    {
        if (sfxToggleButton != null)
        {
            sfxToggleButton.GetComponentInChildren<Text>().text = sfxEnabled ? "SFX: On" : "SFX: Off";
        }
    }
}

