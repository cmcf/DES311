using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    private bool sfxEnabled = true; // Field to store SFX enable/disable state
    private bool musicEnabled = true;

    void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            // Add audio source to game object with sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        // Apply stored SFX and music status
        ApplySettings();
        PlayGameMusic();
    }

    void ApplySettings()
    {
        ApplySFXStatus();
        ApplyMusicStatus();
    }

    void PlayGameMusic()
    {
        if (musicEnabled)
        {
            Play("Music");
        }
    }

    // Method to toggle SFX on/off
    public void ToggleSFX(bool enabled)
    {
        sfxEnabled = enabled;
        PlayerPrefs.SetInt("SFXEnabled", enabled ? 1 : 0); // Save SFX status
        ApplySFXStatus(); // Update SFX status immediately
    }

    // Method to apply SFX status (enabled/disabled) from PlayerPrefs
    public void ApplySFXStatus()
    {
        // Get SFX status from PlayerPrefs (default to enabled if not set)
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        foreach (Sound s in sounds)
        {
            // Set volume based on enable/disable status
            s.source.volume = sfxEnabled ? s.volume : 0.0f;
        }
    }

    // Method to toggle music on/off
    public void ToggleMusic(bool enabled)
    {
        musicEnabled = enabled;
        PlayerPrefs.SetInt("MusicEnabled", enabled ? 1 : 0); // Save music status
        ApplyMusicStatus(); // Update music status immediately
    }

    // Method to apply music status (enabled/disabled) from PlayerPrefs
    public void ApplyMusicStatus()
    {
        // Get music status from PlayerPrefs (default to enabled if not set)
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        Sound music = Array.Find(sounds, sound => sound.type == SoundType.Music);
        if (music != null)
        {
            // Set volume based on music status
            music.source.volume = musicEnabled ? music.volume : 0.0f;
        }
    }

    // Method to play a sound by name
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        // Play the audio clip
        s.source.Play();
    }
}

