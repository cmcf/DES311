using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

     void Awake()
     {
        foreach (Sound s in sounds)
        {
            // Adds audio source to game object with sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
     }
     void Start()
     {
        //Play("Main Theme");
        //Play("Ambient");
     }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // Plays audio clip
        s.source.Play();
    }
}

