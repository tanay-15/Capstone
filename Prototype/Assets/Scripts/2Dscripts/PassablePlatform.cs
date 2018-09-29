using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Physics2D.IgnoreLayerCollision(11,12);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
