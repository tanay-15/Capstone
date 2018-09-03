using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public bool down;

	// Use this for initialization
	void Start () {
        down = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider c)
    {
        Debug.Log("entered");
        if(c.gameObject.tag != "ground")
        {
            down = true;
        }
    }

    private void OnTriggerStay(Collider c)
    {
        Debug.Log("stayed");
        if (c.gameObject.tag != "ground")
        {
            down = true;
        }
    }
    private void OnTriggerExit(Collider c)
    {
        Debug.Log("left");
        if(c.gameObject.tag != "ground")
        {
            down = false;
        }
    }
}
