using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsFunctions : MonoBehaviour
{
    [SerializeField] AudioClip buttonSFX;
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Canvas deathCanvas;

    [Header("Buttons")]
    [SerializeField] Button vibrationOn; // Reference to the vibration button
    [SerializeField] Button vibrationOff; // Reference to the vibration button
    [SerializeField] Button fixedButton; // Reference to the fixed joystick button
    [SerializeField] Button dynamicButton; // Reference to the dynamic joystick button
    [SerializeField] Button sfxOnButton;
    [SerializeField] Button sfxOffButton;

    public bool sfxEnabled = true;
    bool vibrationEnabled = true; // Default vibration state
    bool isFixedJoystickSelected = true; // Default joystick type selection


    void LoadSettings()
    {
        // Load saved settings (vibration and SFX state) from PlayerPrefs
        vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        isFixedJoystickSelected = PlayerPrefs.GetInt("IsFixedJoystickSelected", 1) == 1;
    }

    void SaveSettings()
    {
        // Save current settings (vibration and SFX state) to PlayerPrefs
        PlayerPrefs.SetInt("VibrationEnabled", vibrationEnabled ? 1 : 0);
        PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
        PlayerPrefs.SetInt("IsFixedJoystickSelected", isFixedJoystickSelected ? 1 : 0);
    }


    void UpdateButtonAppearance()
    {
        // Update appearance of vibration button
        if (vibrationEnabled)
        {
            vibrationOn.GetComponent<Image>().color = Color.white;
            vibrationOff.GetComponent<Image>().color = Color.grey;
        }
        else if (!vibrationEnabled)
        {
            vibrationOn.GetComponent<Image>().color = Color.grey;
            vibrationOff.GetComponent<Image>().color = Color.white;
        }
        // Update appearance of joystick type buttons
        if (isFixedJoystickSelected)
        {
            fixedButton.GetComponent<Image>().color = Color.white;
            dynamicButton.GetComponent<Image>().color = Color.grey;
        }
        else if (!isFixedJoystickSelected)
        {
            fixedButton.GetComponent<Image>().color = Color.grey;
            dynamicButton.GetComponent<Image>().color = Color.white;
        }
        // Update appearance of SFX button
        if (sfxEnabled)
        {
            sfxOnButton.GetComponent<Image>().color = Color.white;
            sfxOffButton.GetComponent<Image>().color = Color.grey;
        }
        else if (!sfxEnabled)
        {
            sfxOnButton.GetComponent<Image>().color = Color.grey;
            sfxOffButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void ToggleVibration()
    {
        vibrationEnabled = !vibrationEnabled;
        SaveSettings();
        UpdateButtonAppearance();
    }

    public void SelectFixedJoystick()
    {
        fixedButton.interactable = false;
        dynamicButton.interactable = true;
        SaveSettings();
        UpdateButtonAppearance();
    }

    public void SelectDynamicJoystick()
    {
        fixedButton.interactable = true;
        dynamicButton.interactable = false;
        SaveSettings();
        UpdateButtonAppearance();
    }

    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;
        SaveSettings();
        UpdateButtonAppearance();
    }

    void Start()
    {
        // Load saved settings
        LoadSettings();
        LoadSFXState();

        if (vibrationOn || vibrationOff != null)
        {
            UpdateButtonAppearance();
        }

        if (settingsCanvas != null && deathCanvas != null)
        {
            settingsCanvas.enabled = false;
            deathCanvas.enabled = false;
        }
        else
        {
            return;
        }
    }

    // Method to load the saved SFX state from PlayerPrefs
    void LoadSFXState()
    {
        // Default to sfx enabled if not set
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1; 
    }

    // Method to save the current SFX state to PlayerPrefs
    void SaveSFXState()
    {
        PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
    }

    public void ApplyVibrationButton()
    {
        PlayButtonSFX();
        vibrationEnabled = true;
        Settings.instance.ApplyVibration();
        UpdateButtonAppearance();
    }
    public void DisableVibrationButton()
    {
        PlayButtonSFX();
        vibrationEnabled = false;
        Settings.instance.DisableVibration();
        UpdateButtonAppearance();
    }

    public void BackButton()
    {
        PlayButtonSFX();
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayGame()
    {
        PlayButtonSFX();
        SceneManager.LoadScene(1);
    }
    public void SettingsButton()
    {
        PlayButtonSFX();
        SceneManager.LoadScene("Settings");
    }

    void PlayButtonSFX()
    {
        if (sfxEnabled)
        {
            AudioSource.PlayClipAtPoint(buttonSFX, transform.position);
        }
    }
    public void PauseSettingsButton()
    {
        PlayButtonSFX();
        settingsCanvas.enabled = true;
        Time.timeScale = 0f;
    }

    public void ResumeGameButton()
    {
        PlayButtonSFX();
        settingsCanvas.enabled = false;
        Time.timeScale = 1f;
    }

    public void FixedButton()
    {
        PlayButtonSFX();
        isFixedJoystickSelected = true;
        Settings.instance.ApplyFixedJoystick();
        UpdateButtonAppearance();
    }

    public void DynamicButton()
    {
        PlayButtonSFX();
        isFixedJoystickSelected = false;
        Settings.instance.ApplyDynamicJoystick();
        UpdateButtonAppearance();
    }
    public void EnableSFXButton()
    {
        PlayButtonSFX();
        sfxEnabled = true;
        SaveSFXState();
        UpdateButtonAppearance();
    }

    public void DisableSFXButton()
    {
        PlayButtonSFX();
        sfxEnabled = false;
        SaveSFXState();
        UpdateButtonAppearance();
    }
}
