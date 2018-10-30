using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOrbs : MonoBehaviour {

    // Use this for initialization

    private Vector3 moveVector;
    private float descounter = 5f;

    public float movespeed;
	void Start () {

        moveVector = new Vector3(0, -movespeed * Time.deltaTime, 0);
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Translate(moveVector);

        descounter = descounter - Time.deltaTime;
        if(descounter <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hits " + collision.gameObject.name);
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("GetHit", -15);
        }
        Destroy(this.gameObject);
    }
}
