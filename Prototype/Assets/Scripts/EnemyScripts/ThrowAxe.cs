using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxe : MonoBehaviour {

    // Use this for initialization
    //Move in -x

    private Vector3 moveVector;
    
	void Start () {

        moveVector = new Vector3(-5f * Time.deltaTime, 0, 0);
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
        }
        Destroy(this.gameObject);
    }
}
