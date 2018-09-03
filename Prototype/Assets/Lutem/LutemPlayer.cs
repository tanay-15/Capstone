using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LutemPlayer : MonoBehaviour {

    float maxVelocity;
    private Rigidbody2D rB;

	// Use this for initialization
	void Start () {
        rB = GetComponent<Rigidbody2D>();
        maxVelocity = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            rB.AddForce(new Vector2(-1200.0f * Time.deltaTime, 0.0f));
        }

        if (Input.GetKey(KeyCode.D))
        {
            rB.AddForce(new Vector2(1200.0f * Time.deltaTime, 0.0f));
        }

        if (Input.GetKeyDown(KeyCode.W) && transform.parent.GetComponent<Grounded>().grounded)
        {
            // jumping
            rB.AddForce(new Vector2(0.0f, 750.0f));
        }

        if(Mathf.Abs(rB.velocity.x) > maxVelocity)
        {
            rB.velocity = rB.velocity.normalized * maxVelocity;
            
        }
    }
}
