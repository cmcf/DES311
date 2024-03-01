using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {

    }
}
