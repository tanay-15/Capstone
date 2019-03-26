using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;

        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // In array sounds, we are looking for a sound with name passed in function
        if(!s.source.isPlaying)
        {
            s.source.volume = UnityEngine.Random.Range(0.2f, 0.24f);
            s.source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            s.source.Play();
        }
        
    }
}
