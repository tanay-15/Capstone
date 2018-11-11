using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {

    // Use this for initialization

    public bool DemonWorld = false;

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

        if (playermode_Demon.activeSelf)
        {
            DemonWorld = true;
        }
        else if(playermode_Hero.activeSelf)
        {
            DemonWorld = false;
        }

        Switching();

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
