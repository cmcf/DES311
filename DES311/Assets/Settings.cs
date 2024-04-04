using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance = null;
    private const string VibrationKey = "VibrationEnabled";
    private const string JoystickTypeKey = "JoystickType";
    public JoystickType joystickType;
    public bool vibrationOn;

    void Start()
    {
        // Load the saved vibration setting on start
        vibrationOn = PlayerPrefs.GetInt(VibrationKey, 1) == 1; // Default to true if key doesn't exist
        ApplyVibration(); // Apply the loaded setting
    }
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only be one Settings manager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyVibration()
    {
        vibrationOn = true;
        SaveVibrationSetting();
    }

    public void DisableVibration()
    {
        vibrationOn = false;
        SaveVibrationSetting();
    }

    void SaveVibrationSetting()
    {
        int vibrationValue;
        if (vibrationOn)
        {
            vibrationValue = 1;
        }
        else
        {
            vibrationValue = 0;
        }
        PlayerPrefs.SetInt(VibrationKey, vibrationValue);
        PlayerPrefs.Save();
    }

    public void SetJoystickType(JoystickType type)
    {
        joystickType = type;
        SaveJoystickTypeSetting();
        ApplyJoystickType(); // Apply the new joystick type
    }

    private void ApplyJoystickType()
    {
        // Find the VariableJoystick component and set its mode
        VariableJoystick variableJoystick = FindObjectOfType<VariableJoystick>();
        if (variableJoystick != null)
           variableJoystick.SetMode(joystickType);
    }

    void SaveJoystickTypeSetting()
    {
        PlayerPrefs.SetInt(JoystickTypeKey, (int)joystickType);
        PlayerPrefs.Save();
    }
}
