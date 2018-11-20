using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class SoundManager : MonoBehaviour
{

    static public void PlayMusic(GameObject gameobject, AudioClip audio)
    {
        //gameobject.GetComponent<AudioSource>().PlayOneShot(audio);
        var asource = gameobject.GetComponent<AudioSource>();
        asource.clip = audio;
        asource.Play();
        asource.loop = true;
    }

    static public void StopMusic(GameObject gameobj, AudioClip audio)
    {
        var asource = gameobj.GetComponent<AudioSource>();
        //asource.clip = audio;
        asource.Stop();
    }
}
  
