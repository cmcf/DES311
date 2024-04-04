using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsFunctions : MonoBehaviour
{
    [SerializeField] AudioClip buttonSFX;
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Canvas deathCanvas;

    void Start()
    {
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
    public void ApplyButton()
    {
        PlayButtonSFX();
        Settings.instance.ApplyVibration();
    }
    public void DisableButton()
    {
        PlayButtonSFX();
        Settings.instance.DisableVibration();
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
        AudioSource.PlayClipAtPoint(buttonSFX, transform.position);
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
        Settings.instance.ApplyFixedJoystick();
    }

    public void DynamicButton()
    {
        Settings.instance.ApplyDynamicJoystick();
    }


}
