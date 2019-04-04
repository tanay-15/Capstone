using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    string sceneName;
    bool keepFadingIn = false;
    bool keepFadingOut = false;
    float maxVolume = 1;
    float minVolume = 0;
    float speed = 0.1f;

    public static AudioManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;

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

    private void Start()
    {
        if ((SceneManager.GetSceneByName("NewLevel1").isLoaded))
        {
            PlayMusic("Level1Music");
        }

        else if ((SceneManager.GetSceneByName("Tutorial1").isLoaded))
        {
            PlayMusic("TutorialMusic");
        }

        else if ((SceneManager.GetSceneByName("Tutorial2").isLoaded))
        {
           
            PlayMusic("TutorialMusic");
        }

        else if ((SceneManager.GetSceneByName("MainMenuBasic").isLoaded))
        {

        }

        else if ((SceneManager.GetSceneByName("HubWorld").isLoaded))
        {

        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(!s.source.isPlaying)
        {
            s.source.loop = true;
            s.source.Play();
            StartCoroutine(FadeIn(s.source));

        }
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
        {
            StartCoroutine(FadeOut(s.source));
            s.source.Stop();
        }
    }

    public void PauseMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
        {
            StartCoroutine(FadeOut(s.source));
            s.source.Pause();
        }
    }



    IEnumerator FadeIn(AudioSource source)
    {
        source.volume = 0;
        keepFadingIn = true;
        keepFadingOut = false;
        float audioVolume = source.volume;
        while (source.volume < maxVolume && keepFadingIn)
        {
            audioVolume += speed * Time.deltaTime;
            source.volume = audioVolume;
            yield return new WaitForSeconds(0.05f);
        }


    }

    IEnumerator FadeOut(AudioSource source)
    {
        keepFadingIn = false;
        keepFadingOut = true;
        float audioVolume = source.volume;
        while (source.volume >= minVolume && keepFadingOut)
        {
            audioVolume -= speed * Time.deltaTime;
            source.volume = audioVolume;
            yield return new WaitForSeconds(0.05f);
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
