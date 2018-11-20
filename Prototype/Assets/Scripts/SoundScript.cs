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
    public bool alreadyPlayed = false;
    private bool inCalmZone = true;
    private bool inAlertZone = false;
    private bool inDangerZone = false;
   
	// Use this for initialization
	void Start () {
       // SoundManager.PlayMusic(gameObject, Background_Music[0]);
	}

    // Update is called once per frame
    void Update()
    {
        if (playExploreMusic)
        {
            if (!alreadyPlayed)
            {
                SoundManager.PlayMusic(gameObject, Background_Music[1]);
                alreadyPlayed = true;
            }

        }

        if (stopExploreMusic)
        {
            if (alreadyPlayed)
            {
                SoundManager.StopMusic(gameObject, Background_Music[1]);
                alreadyPlayed = false;
            }
        }

        if (playCombatMusic)
        {
            if (!alreadyPlayed)
            {
                SoundManager.PlayMusic(gameObject, Combat_Music[0]);
                alreadyPlayed = true;
            }
        }

        if (stopCombatMusic)
        {
            if (alreadyPlayed)
            {
                SoundManager.StopMusic(gameObject, Combat_Music[0]);
                alreadyPlayed = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        //if(coll.gameObject.name == "ExploreTrigger")
        //{
      
        //}

        if (coll.gameObject.name == "CombatTrigger")
        {

        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.name == "ExploreTrigger")
        {
            stopExploreMusic = false;         
            playExploreMusic = true;
            playCombatMusic = false;
            stopCombatMusic = false;

        }

        if (coll.gameObject.name == "CombatTrigger")
        {
            playExploreMusic = false;
            playCombatMusic = true;
            stopCombatMusic = false;
            stopExploreMusic = false;
           
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name == "ExploreTrigger")
        {
            playExploreMusic = false;
            stopExploreMusic = true;
           // alreadyPlayed = false;
        }
        if (coll.gameObject.name == "CombatTrigger")
        {
            playCombatMusic = false;
            stopCombatMusic = true;
           // alreadyPlayed = false;
        }
    }
}
