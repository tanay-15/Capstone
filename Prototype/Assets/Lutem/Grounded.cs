using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour {

    public bool grounded;

	// Use this for initialization
	void Start () {
        grounded = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*    private void OnTriggerEnter2D(Collider2D collision)
        {
            grounded = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            grounded = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            grounded = false;
        }*/

    private void OnTriggerEnter(Collider collision)
    {
        grounded = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        grounded = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        grounded = false;
    }
}
