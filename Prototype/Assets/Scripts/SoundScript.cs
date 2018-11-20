using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {
    public GameObject player;
    public GameObject enemy;
    public List<AudioClip> Background_Music;
    public List<AudioClip> Combat_Music;

    public bool playExploreMusic = false;
    public bool playCombatMusic = false;
    public bool stopExploreMusic = false;
    public bool stopCombatMusic = false;
    public bool alreadyPlayedE = false;
    public bool alreadyPlayedC = false;
  
   
	// Use this for initialization
	void Start () {
       // SoundManager.PlayMusic(gameObject, Background_Music[0]);
	}

    // Update is called once per frame
    void Update()
    {
        if (!playExploreMusic && !alreadyPlayedE)
        {
            Debug.Log("Playing");
        }
        if (playExploreMusic)
        {
            if (!alreadyPlayedE)
            {
                SoundManager.PlayMusic(gameObject, Background_Music[1]);
                alreadyPlayedE = true;
            }

        }

        if (stopExploreMusic)
        {
            if (alreadyPlayedE)
            {
                SoundManager.StopMusic(gameObject, Background_Music[1]);
                alreadyPlayedE = false;
            }
        }

        if (playCombatMusic)
        {
            if (!alreadyPlayedC)
            {
                SoundManager.PlayMusic(gameObject, Combat_Music[0]);
                alreadyPlayedC = true;
            }
        }

        if (stopCombatMusic)
        {
            if (alreadyPlayedC)
            {
                SoundManager.StopMusic(gameObject, Combat_Music[0]);
                alreadyPlayedC= false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        //if(coll.gameObject.name == "ExploreTrigger")
        //{
      
        //}

       
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "ExploreTrigger")
        {
            stopExploreMusic = false;         
            playExploreMusic = true;
            playCombatMusic = false;
            stopCombatMusic = false;
            alreadyPlayedC = false;
        }

        if (coll.gameObject.tag == "CombatTrigger")
        {
            playExploreMusic = false;
            playCombatMusic = true;
            stopCombatMusic = false;
            stopExploreMusic = false;
            alreadyPlayedE = false;

        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "ExploreTrigger")
        {
            playExploreMusic = false;
            stopExploreMusic = true;
           // alreadyPlayed = false;
        }
        if (coll.gameObject.tag == "CombatTrigger")
        {
            playCombatMusic = false;
            stopCombatMusic = true;
           // alreadyPlayed = false;
        }
    }
}
