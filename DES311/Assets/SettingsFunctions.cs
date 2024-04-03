using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsFunctions : MonoBehaviour
{
    [SerializeField] AudioClip buttonSFX;
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


}
