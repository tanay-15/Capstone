using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThrough : MonoBehaviour {

    BoxCollider2D collider;
    BoxCollider2D trigger;

	// Use this for initialization
	void Start () {
        collider = GetComponents<BoxCollider2D>()[0];
        trigger = GetComponents<BoxCollider2D>()[1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collider.enabled = true;
        }
    }


}
