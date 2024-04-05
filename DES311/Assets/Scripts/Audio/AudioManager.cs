using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private bool sfxEnabled = true; // Field to store SFX enable/disable state
    private bool musicEnabled = true;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            // Add audio source to game object with sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        // Apply stored SFX and music status
        ApplySFXStatus();
        ApplyMusicStatus();
        PlayGameMusic();
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
        foreach (Sound s in sounds)
        {
            // Set volume based on enable/disable status
            s.source.volume = enabled ? s.volume : 0.0f;
        }
    }

    // Method to apply SFX status (enabled/disabled) from PlayerPrefs
    public void ApplySFXStatus()
    {
        // Get SFX status from PlayerPrefs (default to enabled if not set)
        bool enabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        ToggleSFX(enabled);
    }

    // Method to toggle music on/off
    public void ToggleMusic(bool enabled)
    {
        musicEnabled = enabled;
        Sound music = Array.Find(sounds, sound => sound.type == SoundType.Music);
        if (music != null)
        {
            music.source.volume = enabled ? music.volume : 0.0f;
        }
    }

    // Method to apply music status (enabled/disabled) from PlayerPrefs
    public void ApplyMusicStatus()
    {
        // Get music status from PlayerPrefs (default to enabled if not set)
        bool enabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        ToggleMusic(enabled);
    }

    // Method to check if SFX is enabled
    public bool IsSFXEnabled()
    {
        return sfxEnabled;
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

