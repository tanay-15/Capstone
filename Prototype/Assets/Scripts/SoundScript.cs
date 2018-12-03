using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {
    public GameObject player;
    public List<AudioClip> Background_Music;
    public List<AudioClip> Combat_Music;

    public bool playExploreMusic = false;
    public bool playCombatMusic = false;
    public bool stopExploreMusic = false;
    public bool stopCombatMusic = false;
    public bool alreadyPlayedE = false;
    public bool alreadyPlayedC = false;
    SoundManager soundmanagerScript;
   
	// Use this for initialization
	void Start () {
       
        soundmanagerScript = GetComponent<SoundManager>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!playExploreMusic && !alreadyPlayedE)
        {
            
           
        }
        if (playExploreMusic)
        {
            if (!alreadyPlayedE)
            {
                Debug.Log("Playing");
                soundmanagerScript.PlayMusic(gameObject, Background_Music);
                alreadyPlayedE = true;
            }

        }

        if (stopExploreMusic)
        {
            if (alreadyPlayedE)
            {
                soundmanagerScript.StopMusic(gameObject, Background_Music);
                alreadyPlayedE = false;
            }
        }

        if (playCombatMusic)
        {
            if (!alreadyPlayedC)
            {
                soundmanagerScript.PlayMusic(gameObject, Combat_Music);
                alreadyPlayedC = true;
            }
        }

        if (stopCombatMusic)
        {
            if (alreadyPlayedC)
            {
                soundmanagerScript.StopMusic(gameObject, Combat_Music);
                alreadyPlayedC = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
       
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
        }
        if (coll.gameObject.tag == "CombatTrigger")
        {
            playCombatMusic = false;
            stopCombatMusic = true;
        }
    }

   
}
