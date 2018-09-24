﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonModeScript : MonoBehaviour {

    public GameObject bat;
    public bool DemonModeActive;
    bool transitioning;
    public Camera MainCam;

	// Use this for initialization
	void Start () {
        DemonModeActive = false;
        transitioning = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("z") && !transitioning)
        {
            transitioning = true;
            StartCoroutine("Transform", !DemonModeActive);
            for (int i = 0; i < 50; i++)
            {
                var Bat = Instantiate(bat, transform.position + new Vector3(0,100,0), Quaternion.identity);
                Bat.GetComponent<Bat_Script>().player = gameObject;
            }

            if (!DemonModeActive)
            {
                GetComponent<AudioSource>().Play();
                GetComponent<Movement2D>().speed = 7;               

            }
            else
            {
                GetComponent<Movement2D>().speed = 5;              
            }
                
        }

        if (transitioning)
        {
            if(!DemonModeActive)
                MainCam.backgroundColor = new Color(MainCam.backgroundColor.r-0.002f, MainCam.backgroundColor.g - 0.002f, MainCam.backgroundColor.b - 0.003f, MainCam.backgroundColor.a);
            else
                MainCam.backgroundColor = new Color(MainCam.backgroundColor.r + 0.002f, MainCam.backgroundColor.g + 0.002f, MainCam.backgroundColor.b + 0.003f, MainCam.backgroundColor.a);
        }

	}

    IEnumerator Transform(bool toDemon)
    {
        yield return new WaitForSeconds(2.7f);

        transform.GetComponent<Movement2D>().myAnim = (toDemon) ? transform.Find("Demon").GetComponent<Animator>() : transform.Find("Normal").GetComponent<Animator>();
        transform.Find("Normal").gameObject.SetActive(!toDemon);
        transform.Find("Demon").gameObject.SetActive(toDemon);
        transitioning = false;
        DemonModeActive = toDemon;
    }
}
