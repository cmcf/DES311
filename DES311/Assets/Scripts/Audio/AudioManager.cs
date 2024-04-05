using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public Sound[] sounds;
    private bool sfxEnabled = true; // New field to store SFX enable/disable state

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            // Adds audio source to game object with sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            // Apply stored SFX status
            ApplySFXStatus();

        }
    }

    public void UpdateMusicVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            if (s.type == SoundType.Music)
            {
                s.source.volume = volume;
            }
        }
    }

    public void ToggleSFX(bool enabled)
    {
        sfxEnabled = enabled;
        foreach (Sound s in sounds)
        {
            if (s.type == SoundType.SFX)
            {
                s.source.volume = enabled ? s.volume : 0.0f;
            }
        }
    }
    public void ApplySFXStatus()
    {
        bool enabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1; // Default to enabled if not set
        ToggleSFX(enabled);
    }

    public bool IsSFXEnabled()
    {
        return sfxEnabled;
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        // Plays audio clip
        if (s != null && s.source != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }
}

