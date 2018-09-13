using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDoorStatus : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void toggle()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;
    }
}
