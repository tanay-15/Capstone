using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKnife2D : MonoBehaviour {

    bool hasHit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!hasHit == true)
            transform.Rotate(new Vector3(0,0,-Time.deltaTime * 250));

	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Destroy(gameObject);
        if(collision.gameObject.name == "Torso" || collision.gameObject.name == "Leg" || collision.gameObject.name == "Head" || collision.gameObject.name == "LeftLeg")
        {
            hasHit = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            transform.parent = collision.gameObject.transform;
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Torso")
        {
            hasHit = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            transform.parent = collision.gameObject.transform;
        }
    }
    */
}
