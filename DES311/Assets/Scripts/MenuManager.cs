using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonSFX;
    public void PlayGameButton()
    {
        Debug.Log("Play button pressed");
        PlayButtonSFX();
        SceneManager.LoadScene(1);
    }

    public void QuitGameButton()
    {
        PlayButtonSFX();
        Application.Quit();
    }

    public void SettingsButton()
    {
        PlayButtonSFX();
        SceneManager.LoadScene("Settings");
    }

    public void MainMenu()
    {
        PlayButtonSFX();
        SceneManager.LoadScene("MainMenu");
    }

    void PlayButtonSFX()
    {
        AudioSource.PlayClipAtPoint(buttonSFX, transform.position);
    }
}
