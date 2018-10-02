using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSmash : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Translate(-Vector3.right * 5f * Time.deltaTime);
	}


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Grabbable")
        {
            Destroy(this.gameObject);
        }
    }
}
