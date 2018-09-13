using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }
    
    public void toggle()
    {
        if(this.gameObject.GetComponent<SpriteRenderer>().color == Color.red)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
