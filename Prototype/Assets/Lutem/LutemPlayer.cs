using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LutemPlayer : MonoBehaviour {

    float maxVelocity;
    private Rigidbody rB;

	// Use this for initialization
	void Start () {
        rB = GetComponent<Rigidbody>();
        maxVelocity = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            rB.AddForce(new Vector3(-3600.0f * Time.deltaTime, 0.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.D))
        {
            rB.AddForce(new Vector3(3600.0f * Time.deltaTime, 0.0f, 0.0f));
        }

        if (Input.GetKeyDown(KeyCode.W) && transform.GetChild(0).GetComponent<Grounded>().grounded)
        {
            // jumping
            rB.AddForce(new Vector3(0.0f, 750.0f, 0.0f));
        }

        if(Mathf.Abs(rB.velocity.x) > maxVelocity)
        {
            rB.velocity = new Vector3(rB.velocity.normalized.x * maxVelocity, rB.velocity.y, 0.0f);
            
        }
    }
}
