using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    AudioSource asource;
    bool keepFadingIn = false;
    bool keepFadingOut = false;
    float trackLength;
    int i;
    float maxVolume =1;
    float minVolume = 0;
    float speed =0.1f;
    public void PlayMusic(GameObject gameobject, List<AudioClip> audio)
    {
        Debug.Log("play music");
        //gameobject.GetComponent<AudioSource>().PlayOneShot(audio);
        asource = gameobject.GetComponent<AudioSource>();
        
        //for (i = 0; i < audio[i].length; i++)
        foreach(AudioClip clip in audio)
        {
            asource.clip = clip;
            Debug.Log("Before FADE IN");
            asource.Play();
            StartCoroutine(FadeIn(gameobject, asource.clip));
           
            asource.loop = true;
         
        }
    }

    public void StopMusic(GameObject gameobj, List<AudioClip> audio)
    {
        asource = gameobj.GetComponent<AudioSource>();
        StartCoroutine(FadeOut(gameobj, asource.clip));
        //asource.Stop();
        
    }
    IEnumerator FadeIn(GameObject obj, AudioClip audio)
    {
        Debug.Log("Inside Fade in");
        asource = obj.GetComponent<AudioSource>();       
        asource.clip = audio;
        Debug.Log("before making volume 0");
        asource.volume = 0;
        keepFadingIn = true;
        keepFadingOut = false;       
        float audioVolume = asource.volume;
        while(asource.volume < maxVolume && keepFadingIn)
        {
            audioVolume += speed * Time.deltaTime;
            asource.volume = audioVolume;
            yield return new WaitForSeconds(0.05f);
        }

        
    }

    IEnumerator FadeOut(GameObject obj, AudioClip audio)
    {
        asource = obj.GetComponent<AudioSource>();
        keepFadingIn = false;
        keepFadingOut = true;
        float audioVolume = asource.volume;
        while (asource.volume >= minVolume && keepFadingOut)
        {
            audioVolume -= speed * Time.deltaTime;
            asource.volume = audioVolume;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
  
