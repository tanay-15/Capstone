using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.pitch;
            s.source.playOnAwake = false;

        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // In array sounds, we are looking for a sound with name passed in function
        if(!s.source.isPlaying)
        {
            s.volume = UnityEngine.Random.Range(0.3f, 0.6f);
            s.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
            s.source.Play();
        }
        
    }
}
