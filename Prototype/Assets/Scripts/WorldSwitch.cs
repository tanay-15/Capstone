using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {

    // Use this for initialization

    public bool DemonWorld = false;
    public float demonModeCounter;
    public bool startDemonModeCounter;


    //***************************Meteor****************

    public GameObject MeteorObject;
    public GameObject MetPosition;

    //***********************
    public GameObject player;
    public GameObject playermode_Hero;
    public GameObject playermode_Demon;

    public Sprite sprite1;
    public Sprite sprite2;

    public GameObject[] walls;

	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        walls = GameObject.FindGameObjectsWithTag("Wall");
	}
	
	// Update is called once per frame
	void Update () {

        CheckingForPlayerMode();
        Switching();
        DemonModeEnvironment();

	}

    void DemonModeEnvironment()
    {
        if (startDemonModeCounter)
        {
            demonModeCounter = demonModeCounter + Time.deltaTime;

            if(demonModeCounter >= 5f)
            {
                Meteor_Shower();
                demonModeCounter = 0;
            }
        }
    }


    void Meteor_Shower()
    {
        Instantiate(MeteorObject, MetPosition.transform.position, MetPosition.transform.rotation);
    }

    void CheckingForPlayerMode()
    {

        if (playermode_Demon.activeSelf)
        {
            DemonWorld = true;
            startDemonModeCounter = true;
        }
        else if (playermode_Hero.activeSelf)
        {
            DemonWorld = false;
            startDemonModeCounter = false;
        }
    }

    void Switching()
    {
        if (DemonWorld)
        {
            foreach(GameObject go in walls)
            {
                go.GetComponent<SpriteRenderer>().sprite = sprite2;
            }
        }

        if (!DemonWorld)
        {
            foreach(GameObject go in walls)
            {
                go.GetComponent<SpriteRenderer>().sprite = sprite1;
            }
        }
    }
}
