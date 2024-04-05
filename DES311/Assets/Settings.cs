using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings instance = null;
    [Header("Vibration")]

    private const string VibrationKey = "VibrationEnabled";
    public bool vibrationOn;

    [Header("Joystick")]

    public VariableJoystick variableJoystick;
    private const string JoystickTypeKey = "JoystickType";
    public JoystickType joystickType;


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
        // Saves vibration on setting
        vibrationOn = true;
        SaveVibrationSetting();
    }

    public void DisableVibration()
    {
        // Disables vibration
        vibrationOn = false;
        SaveVibrationSetting();
    }

    void SaveVibrationSetting()
    {
        // Saves players vibration preference
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

    public void ApplyFixedJoystick()
    {
        SaveJoystickValue(JoystickType.Fixed);
    }

    public void ApplyDynamicJoystick()
    {
        SaveJoystickValue(JoystickType.Dynamic);  
    }
    void SaveJoystickValue(JoystickType joystickType)
    {
        // Save the selected joystick type setting to PlayerPrefs
        PlayerPrefs.SetInt(JoystickTypeKey, (int)joystickType);
        PlayerPrefs.Save();
    }

    public JoystickType GetAppliedJoystickType()
    {
        // Retrieve the saved joystick type setting from PlayerPrefs
        int joystickTypeValue = PlayerPrefs.GetInt(JoystickTypeKey, (int)JoystickType.Dynamic);
        return (JoystickType)joystickTypeValue;
    }

}
