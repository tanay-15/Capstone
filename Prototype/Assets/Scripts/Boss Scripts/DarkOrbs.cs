using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOrbs : MonoBehaviour {

    // Use this for initialization

    private Vector3 moveVector;
	void Start () {

        moveVector = new Vector3(-5 * Time.deltaTime, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Translate(moveVector);
	}

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hits " + collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
