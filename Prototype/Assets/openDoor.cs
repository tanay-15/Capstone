using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "key")
        {
            ///this.GetComponent<SpriteRenderer>().enabled = false;
            //this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            //this.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.SetActive(false);
        }
    }
}
