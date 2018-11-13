using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour {

    // Use this for initialization

    public float movSpeed = 6f;
    public Vector3 moveVector;
	void Start () {


        moveVector = new Vector3(0, -movSpeed * Time.deltaTime, 0);
	}
	
	// Update is called once per frame
	void Update () {

        //Making the meteor fall down
        this.transform.Translate(moveVector);

	}

    void OnCollisionEnter2D(Collision2D collision)
    {
       // Destroy(this.gameObject);
    }


}
