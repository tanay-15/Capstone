﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxe : MonoBehaviour {

    // Use this for initialization
    //Move in -x

    private Vector3 moveVector;
    
	void Start () {

        //moveVector = new Vector3(-8f * Time.deltaTime, 0, 0);
        moveVector = 8*Vector3.Normalize(GameObject.Find("Character").transform.position- transform.position);

    }
	
	// Update is called once per frame
	void Update () {

        this.transform.Translate(moveVector);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHit", -15);
            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "projectile")
        {
            Destroy(this.gameObject);
        }
        else
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
        
    }
}
