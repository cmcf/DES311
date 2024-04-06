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

    public void ToggleSFX(bool enabled)
    {
        sfxEnabled = enabled;
        // Save SFX status
        if (enabled)
        {
            PlayerPrefs.SetInt("SFXEnabled", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SFXEnabled", 0);
        }
        // Update SFX state
        ApplySFXStatus();
    }

    public void ApplySFXStatus()
    {
        // Get SFX status from PlayerPrefs. Set to enabled if not set. 
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        foreach (Sound s in sounds)
        {
            // Check if the sound is SFX
            if (s.type == SoundType.SFX)
            {
                // Set volume based on enable/disable state
                s.source.volume = sfxEnabled ? s.volume : 0.0f;
            }
        }
    }
    public void ToggleMusic(bool enabled)
    {
        musicEnabled = enabled;
        if (enabled)
        {
            PlayerPrefs.SetInt("MusicEnabled", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MusicEnabled", 0);
        }
        // Update music state
        ApplyMusicStatus(); 
    }

    public void ApplyMusicStatus()
    {
        // Get music status from PlayerPrefs. Set to enabled if not set. 
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        Sound music = Array.Find(sounds, sound => sound.type == SoundType.Music);
        if (music != null)
        {
            // Set volume based on music status
            music.source.volume = musicEnabled ? music.volume : 0.0f;
        }
    }

    public void Play(string name)
    {
        // Plays sound based on name
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

